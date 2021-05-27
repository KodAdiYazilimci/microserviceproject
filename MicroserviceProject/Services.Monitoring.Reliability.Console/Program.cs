using Infrastructure.Communication.Mq.Rabbit.Models;
using Infrastructure.Communication.WebSockets;
using Infrastructure.Mock.Factories;

using Microsoft.Extensions.Configuration;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Monitoring.Reliability.Console
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
                socketName: SocketNameProviderFactory.GetSocketNameProvider(GetConfiguration(args)).Reliability_ErrorHub_GetErrorMessages,
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
