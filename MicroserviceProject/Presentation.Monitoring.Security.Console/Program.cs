using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Communication.WebSockets;
using Infrastructure.Communication.WebSockets.Models;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Security.Authentication.Mock;
using Infrastructure.Sockets.Persistence.Mock;

using Microsoft.Extensions.Configuration;

using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Monitoring.Security.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            SocketListener socketListener = new SocketListener(
                cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                credentialProvider: CredentialProviderFactory.GetCredentialProvider(GetConfiguration(args)),
                serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(GetConfiguration(args)),
                socketRepository: SocketRepositoryFactory.GetSocketRepository(GetConfiguration(args)),
                serviceCaller: ServiceCallerFactory.GetServiceCaller(HttpClientFactory.Instance, InMemoryCacheDataProviderFactory.Instance));

            socketListener.OnMessageReceived += (WebSocketResultModel webSocketResult) =>
            {
                System.Console.WriteLine(webSocketResult.Content.Message);
            };

            await socketListener.ListenAsync(
                socketName: "websockets.security.tokenshub.gettokenmessages",
                cancellationTokenSource: cancellationTokenSource);

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
