
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Services.Communication.Mq.Queue.IT.Rabbit.Consumers;

namespace Services.MQ.IT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            ITAssignInventoryToWorkerConsumer assignInventoryToWorkerConsumer =
                    (ITAssignInventoryToWorkerConsumer)host.Services.GetService(typeof(ITAssignInventoryToWorkerConsumer));

            assignInventoryToWorkerConsumer.StartToConsume();

            ITInformInventoryRequestConsumer informInventoryRequestConsumer =
                (ITInformInventoryRequestConsumer)host.Services.GetService(typeof(ITInformInventoryRequestConsumer));

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
