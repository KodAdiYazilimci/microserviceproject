using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Services.Api.Infrastructure.Logging.Util.Logging.Consumers;

using System.Threading;

namespace Services.Api.Infrastructure.Logging.DI
{
    /// <summary>
    /// Rabbit kuyruk tüketici sınıfların DI sınıfı
    /// </summary>
    public static class ConsumerConfiguration
    {
        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="hostBuilder">Hosting nesnesi</param>
        /// <returns></returns>
        public static IHost RegisterConsumers(this IHost hostBuilder)
        {
            IConfiguration configuration =
                (IConfiguration)hostBuilder.Services.GetService(typeof(IConfiguration));

            RequestResponseLogConsumer requestResponseLogConsumer = new RequestResponseLogConsumer(configuration);
            requestResponseLogConsumer.StartConsumeAsync(new CancellationTokenSource());

            return hostBuilder;
        }
    }
}
