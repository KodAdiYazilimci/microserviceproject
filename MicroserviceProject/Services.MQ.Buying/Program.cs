
using Services.MQ.Buying.Util.Consumers.Cost;
using Services.MQ.Buying.Util.Consumers.Request;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Services.MQ.Buying
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateInventoryRequestConsumer createInventoryRequestConsumer =
                    (CreateInventoryRequestConsumer)host.Services.GetService(typeof(CreateInventoryRequestConsumer));

            createInventoryRequestConsumer.StartToConsume();

            NotifyCostApprovementConsumer notifyCostApprovementConsumer =
                (NotifyCostApprovementConsumer)host.Services.GetService(typeof(NotifyCostApprovementConsumer));

            notifyCostApprovementConsumer.StartToConsume();

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
