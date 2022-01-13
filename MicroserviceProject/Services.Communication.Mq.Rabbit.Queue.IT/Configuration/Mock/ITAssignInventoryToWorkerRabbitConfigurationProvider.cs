
using Microsoft.Extensions.Configuration;

namespace Services.Communication.Mq.Rabbit.Queue.IT.Configuration.Mock
{
    /// <summary>
    /// Çalışana envanter atayan IT yayıncısının yapılandırma sınıfını taklit eden sınıf
    /// </summary>
    public class ITAssignInventoryToWorkerRabbitConfigurationProvider
    {
        /// <summary>
        /// Çalışana envanter atayan IT yayıncısının yapılandırma sınıfı
        /// </summary>
        private static AssignInventoryToWorkerRabbitConfiguration rabbitConfiguration = null;

        /// <summary>
        /// Çalışana envanter atayan IT yayıncısının yapılandırma sınıfı nesnesini verir
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
