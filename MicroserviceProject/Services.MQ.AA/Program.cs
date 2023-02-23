
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Services.Communication.Mq.Queue.AA.Rabbit.Consumers;

namespace Services.MQ.AA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            AAAssignInventoryToWorkerConsumer assignInventoryToWorkerConsumer =
                    (AAAssignInventoryToWorkerConsumer)host.Services.GetService(typeof(AAAssignInventoryToWorkerConsumer));

            assignInventoryToWorkerConsumer.StartToConsume();

            AAInformInventoryRequestConsumer informInventoryRequestConsumer =
                (AAInformInventoryRequestConsumer)host.Services.GetService(typeof(AAInformInventoryRequestConsumer));

            informInventoryRequestConsumer.StartToConsume();

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
