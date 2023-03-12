using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Services.Communication.Mq.Queue.Authorization.Rabbit.Consumers;

using System.Threading;

namespace Services.MQ.Authorization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            InformInvalidTokenConsumer informInvalidTokenConsumer =
                    (InformInvalidTokenConsumer)host.Services.GetService(typeof(InformInvalidTokenConsumer));

            informInvalidTokenConsumer.StartConsumeAsync(new CancellationTokenSource());

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
