using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Communication.WebSockets;
using Infrastructure.Communication.WebSockets.Models;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Security.Authentication.Abstract;
using Infrastructure.Security.Authentication.Mock;
using Infrastructure.ServiceDiscovery.Discoverer.Mock;
using Infrastructure.ServiceDiscovery.Mock;
using Infrastructure.Sockets.Persistence.Mock;

using Microsoft.Extensions.Configuration;

using Services.Communication.Http.Broker.Authorization;
using Services.Communication.Http.Broker.Authorization.Models;
using Services.Communication.Http.Broker.Mock;

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Monitoring.Security.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = GetConfiguration(args);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            SocketListener socketListener = new SocketListener(
                cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                credentialProvider: CredentialProviderFactory.GetCredentialProvider(configuration),
                serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(configuration),
                socketRepository: SocketRepositoryFactory.GetSocketRepository(configuration));

            socketListener.OnMessageReceived += (WebSocketResultModel webSocketResult) =>
            {
                System.Console.WriteLine(webSocketResult.Content.Message);
            };

            AuthorizationCommunicator authorizationCommunicator =
               new AuthorizationCommunicator
               (
                   communicator: DefaultCommunicatorProvider.GetDefaultCommunicator
                   (
                       httpGetCaller: HttpGetCallerFactory.Instance,
                       httpPostCaller: HttpPostCallerFactory.Instance
                   ),
                   serviceDiscoverer: HttpServiceDiscovererProvider.GetServiceDiscoverer(
                        inMemoryCacheDataProvider: InMemoryCacheDataProviderFactory.Instance,
                        httpGetCaller: HttpGetCallerFactory.Instance,
                        solidServiceProvider: AppConfigSolidServiceProviderProvider.GetSolidServiceConfiguration(configuration),
                        solidServiceConfiguration: AppConfigSolidServiceConfigurationProvider.GetSolidServiceConfiguration(configuration))
               );

            ICredentialProvider credentialProvider = CredentialProviderFactory.GetCredentialProvider(configuration: null);

            ServiceResultModel<TokenModel> token = await authorizationCommunicator.GetTokenAsync(new CredentialModel()
            {
                Email = credentialProvider.GetEmail,
                Password = credentialProvider.GetPassword
            }, cancellationTokenSource);
            while (token != null && token.IsSuccess && token.Data != null && token.Data.ValidTo > DateTime.Now)
            {
                try
                {
                    await socketListener.ListenAsync(
                        socketName: "websockets.security.tokenshub.gettokenmessages",
                        token: token.Data.TokenKey,
                        cancellationTokenSource: cancellationTokenSource);
                }
                catch (WebException wex)
                {
                    if (wex.Response != null)
                    {
                        if (wex.Response is HttpWebResponse && (wex.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                        {
                            token = await authorizationCommunicator.GetTokenAsync(new CredentialModel()
                            {
                                Email = credentialProvider.GetEmail,
                                Password = credentialProvider.GetPassword
                            }, cancellationTokenSource);
                        }
                    }
                }
            }

            System.Console.ReadKey();

        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            IConfiguration configuration =
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            return configuration;
        }
    }
}
