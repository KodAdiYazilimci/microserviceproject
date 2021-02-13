using MicroserviceProject.Services.MQ.AA.DI;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MicroserviceProject.Services.MQ.AA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.RegisterConsumers();

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
