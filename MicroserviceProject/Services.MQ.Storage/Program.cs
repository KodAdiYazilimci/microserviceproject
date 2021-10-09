
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Services.MQ.Storage.Util.Consumers.ProductStock;

namespace Services.MQ.Storage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            DescendProductStockConsumer descendProductStockConsumer =
                    (DescendProductStockConsumer)host.Services.GetService(typeof(DescendProductStockConsumer));

            descendProductStockConsumer.StartToConsume();

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
