using MicroserviceProject.Services.MQ.Accounting.Util.Consumers.Inventory;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MicroserviceProject.Services.MQ.Accounting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateBankAccountConsumer assignInventoryToWorkerConsumer =
                    (CreateBankAccountConsumer)host.Services.GetService(typeof(CreateBankAccountConsumer));

            assignInventoryToWorkerConsumer.StartToConsume();

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
