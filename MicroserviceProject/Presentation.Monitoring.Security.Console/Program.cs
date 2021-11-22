using Infrastructure.Communication.WebSockets;
using Infrastructure.Communication.WebSockets.Models;
using Infrastructure.Mock.Factories;

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
                routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(GetConfiguration(args)),
                serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(GetConfiguration(args)),
                socketNameProvider: SocketNameProviderFactory.GetSocketNameProvider(GetConfiguration(args)),
                socketRepository: SocketRepositoryFactory.GetSocketRepository(GetConfiguration(args)));

            socketListener.OnMessageReceived += (WebSocketResultModel webSocketResult) =>
            {
                System.Console.WriteLine(webSocketResult.Content.Message);
            };

            await socketListener.ListenAsync(
                socketName: SocketNameProviderFactory.GetSocketNameProvider(GetConfiguration(args)).Security_TokensHub_GetTokenMessages,
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
