
using Microsoft.Extensions.Configuration;

namespace Services.Communication.Mq.Queue.AA.Configuration.Mock
{
    /// <summary>
    /// Çalışana envanter atayan idari işler yayıncı yapılandırmasını taklit eden sınıf
    /// </summary>
    public class AAAssignInventoryToWorkerRabbitConfigurationProvider
    {
        /// <summary>
        /// Çalışana envanter atayan idari işler yayıncı yapılandırmasını verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static AssignInventoryToWorkerRabbitConfiguration GetConfiguration(IConfiguration configuration)
        {
            return new AssignInventoryToWorkerRabbitConfiguration(configuration);
        }
    }
}
