using Services.Communication.Mq.Rabbit.Configuration.Department.AA;

using Microsoft.Extensions.Configuration;

namespace Services.Communication.Mq.Rabbit.Publisher.Mock
{
    /// <summary>
    /// Çalışana envanter atayan idari işler yayıncı yapılandırmasını taklit eden sınıf
    /// </summary>
    public class AAAssignInventoryToWorkerRabbitConfigurationProvider
    {
        /// <summary>
        /// Çalışana envanter atayan idari işler yayıncı yapılandırması
        /// </summary>
        private static AssignInventoryToWorkerRabbitConfiguration rabbitConfiguration = null;

        /// <summary>
        /// Çalışana envanter atayan idari işler yayıncı yapılandırmasını verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static AssignInventoryToWorkerRabbitConfiguration GetConfiguration(IConfiguration configuration)
        {
            if (rabbitConfiguration == null)
            {
                rabbitConfiguration = new AssignInventoryToWorkerRabbitConfiguration(configuration);
            }

            return rabbitConfiguration;
        }
    }
}
