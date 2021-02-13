
using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Configuration.Communication.Rabbit.IT
{
    /// <summary>
    /// Çalışana envanter ekleyecek rabbit kuyruğu için yapılandırma sınıfı
    /// </summary>
    public class ITAssignInventoryToWorkerRabbitConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Çalışana envanter ekleyecek rabbit kuyruğu için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public ITAssignInventoryToWorkerRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("IT")
                .GetSection("QueueNames")
                .GetSection("AssignInventoryToWorker").Value;
        }
    }
}
