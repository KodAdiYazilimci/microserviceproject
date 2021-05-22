using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.IT;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Publishers
{
    /// <summary>
    /// Çalışana envanter atayan IT yayıncısının yapılandırma sınıfını taklit eden sınıf
    /// </summary>
    public class ITAssignInventoryToWorkerRabbitConfigurationProvider
    {
        /// <summary>
        /// Çalışana envanter atayan IT yayıncısının yapılandırma sınıfı
        /// </summary>
        private static ITAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration = null;

        /// <summary>
        /// Çalışana envanter atayan IT yayıncısının yapılandırma sınıfı nesnesini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static ITAssignInventoryToWorkerRabbitConfiguration GetConfiguration(IConfiguration configuration)
        {
            if (rabbitConfiguration == null)
            {
                rabbitConfiguration = new ITAssignInventoryToWorkerRabbitConfiguration(configuration);
            }

            return rabbitConfiguration;
        }
    }
}
