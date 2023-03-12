using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Services.Communication.Mq.Queue.Production.Rabbit.Consumers;

using System.Threading;

namespace Services.MQ.Production
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            ProduceConsumer createInventoryRequestConsumer =
                    (ProduceConsumer)host.Services.GetService(typeof(ProduceConsumer));

            createInventoryRequestConsumer.StartConsumeAsync(new CancellationTokenSource());

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
