using Communication.Mq.Rabbit.Configuration.Department.AA;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Publishers
{
    /// <summary>
    /// Çalışana envanter atayan idari işler yayıncı yapılandırmasını taklit eden sınıf
    /// </summary>
    public class AAAssignInventoryToWorkerRabbitConfigurationProvider
    {
        /// <summary>
        /// Çalışana envanter atayan idari işler yayıncı yapılandırması
        /// </summary>
        private static AAAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration = null;

        /// <summary>
        /// Çalışana envanter atayan idari işler yayıncı yapılandırmasını verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static AAAssignInventoryToWorkerRabbitConfiguration GetConfiguration(IConfiguration configuration)
        {
            if (rabbitConfiguration == null)
            {
                rabbitConfiguration = new AAAssignInventoryToWorkerRabbitConfiguration(configuration);
            }

            return rabbitConfiguration;
        }
    }
}
