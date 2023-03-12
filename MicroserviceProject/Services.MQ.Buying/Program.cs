
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Services.Communication.Mq.Queue.Buying.Rabbit.Consumers;

using System.Threading;

namespace Services.MQ.Buying
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateInventoryRequestConsumer createInventoryRequestConsumer =
                    (CreateInventoryRequestConsumer)host.Services.GetService(typeof(CreateInventoryRequestConsumer));

            var cancellationTokenSource = new CancellationTokenSource();

            createInventoryRequestConsumer.StartConsumeAsync(cancellationTokenSource);

            NotifyCostApprovementConsumer notifyCostApprovementConsumer =
                (NotifyCostApprovementConsumer)host.Services.GetService(typeof(NotifyCostApprovementConsumer));

            notifyCostApprovementConsumer.StartConsumeAsync(cancellationTokenSource);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
