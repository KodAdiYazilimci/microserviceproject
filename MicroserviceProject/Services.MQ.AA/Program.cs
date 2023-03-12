
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Services.Communication.Mq.Queue.AA.Rabbit.Consumers;

using System.Threading;

namespace Services.MQ.AA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            AAAssignInventoryToWorkerConsumer assignInventoryToWorkerConsumer =
                    (AAAssignInventoryToWorkerConsumer)host.Services.GetService(typeof(AAAssignInventoryToWorkerConsumer));

            var cancellationTokenSource = new CancellationTokenSource();

            assignInventoryToWorkerConsumer.StartConsumeAsync(cancellationTokenSource);

            AAInformInventoryRequestConsumer informInventoryRequestConsumer =
                (AAInformInventoryRequestConsumer)host.Services.GetService(typeof(AAInformInventoryRequestConsumer));

            informInventoryRequestConsumer.StartToConsumeAsync(cancellationTokenSource);

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
