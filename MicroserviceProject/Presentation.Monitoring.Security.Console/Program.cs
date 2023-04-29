using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Security.Authentication.Abstract;
using Infrastructure.Security.Authentication.Mock;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Mock;
using Infrastructure.ServiceDiscovery.Discoverer.Models;
using Infrastructure.ServiceDiscovery.Mock;
using Infrastructure.Sockets.Models;

using Microsoft.Extensions.Configuration;

using Services.Communication.Http.Broker.Authorization;
using Services.Communication.Http.Broker.Authorization.Models;
using Services.Communication.Http.Broker.Mock;
using Services.Communication.Http.Endpoint.Authorization;
using Services.WebSockets;

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

            IServiceDiscoverer serviceDiscoverer = HttpServiceDiscovererProvider.GetServiceDiscoverer(
                inMemoryCacheDataProvider: InMemoryCacheDataProviderFactory.Instance,
                httpGetCaller: HttpGetCallerFactory.Instance,
                solidServiceProvider: AppConfigSolidServiceProviderProvider.GetSolidServiceConfiguration(configuration),
                discoveryConfiguration: AppConfigDiscoveryConfigurationProvider.GetDiscoveryConfiguration(configuration));

            SocketListener socketListener = new SocketListener(
                credentialProvider: CredentialProviderFactory.GetCredentialProvider(configuration));

            socketListener.OnMessageReceived += (WebSocketResultModel webSocketResult) =>
            {
                System.Console.WriteLine(webSocketResult.Content.Message);
            };

            AuthorizationCommunicator authorizationCommunicator =
               new AuthorizationCommunicator
               (
                   communicator: AuthenticatedCommunicatorProvider.GetAuthenticatedCommunicator
                   (
                       httpGetCaller: HttpGetCallerFactory.Instance,
                       httpPostCaller: HttpPostCallerFactory.Instance
                   ),
                   serviceDiscoverer: serviceDiscoverer
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
                    CachedServiceModel service = await serviceDiscoverer.GetServiceAsync("Services.Api.Authorization", cancellationTokenSource);

                    IEndpoint endpoint = service.GetEndpoint(GetTokenEndpoint.Path);

                    await socketListener.ListenAsync(
                        socketEndpoint: null,
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
