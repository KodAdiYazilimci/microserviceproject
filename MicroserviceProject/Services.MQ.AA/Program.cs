using Services.MQ.AA.Util.Consumers.Inventory;
using Services.MQ.AA.Util.Consumers.Request;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Services.MQ.AA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            AssignInventoryToWorkerConsumer assignInventoryToWorkerConsumer =
                    (AssignInventoryToWorkerConsumer)host.Services.GetService(typeof(AssignInventoryToWorkerConsumer));

            assignInventoryToWorkerConsumer.StartToConsume();

            InformInventoryRequestConsumer informInventoryRequestConsumer =
                (InformInventoryRequestConsumer)host.Services.GetService(typeof(InformInventoryRequestConsumer));

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
