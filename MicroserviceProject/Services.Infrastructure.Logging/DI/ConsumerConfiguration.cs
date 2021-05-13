using Services.Infrastructure.Logging.Util.Logging.Consumers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Services.Infrastructure.Logging.DI
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
            requestResponseLogConsumer.StartToConsume();

            return hostBuilder;
        }
    }
}
