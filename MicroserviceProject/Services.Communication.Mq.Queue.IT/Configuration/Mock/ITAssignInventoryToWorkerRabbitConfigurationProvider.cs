
using Microsoft.Extensions.Configuration;

namespace Services.Communication.Mq.Queue.IT.Configuration.Mock
{
    /// <summary>
    /// Çalışana envanter atayan IT yayıncısının yapılandırma sınıfını taklit eden sınıf
    /// </summary>
    public class ITAssignInventoryToWorkerRabbitConfigurationProvider
    {
        /// <summary>
        /// Çalışana envanter atayan IT yayıncısının yapılandırma sınıfı nesnesini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static AssignInventoryToWorkerRabbitConfiguration GetConfiguration(IConfiguration configuration)
        {
            return new AssignInventoryToWorkerRabbitConfiguration(configuration);
        }
    }
}
