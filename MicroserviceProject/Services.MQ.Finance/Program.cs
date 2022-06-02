
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Services.Communication.Mq.Queue.Finance.Rabbit.Consumers;

namespace Services.MQ.Finance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            InventoryRequestConsumer inventoryRequestConsumer =
                (InventoryRequestConsumer)host.Services.GetService(typeof(InventoryRequestConsumer));

            inventoryRequestConsumer.StartToConsume();

            ProductionRequestConsumer productionRequestConsumer =
                (ProductionRequestConsumer)host.Services.GetService(typeof(ProductionRequestConsumer));

            productionRequestConsumer.StartToConsume();

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
