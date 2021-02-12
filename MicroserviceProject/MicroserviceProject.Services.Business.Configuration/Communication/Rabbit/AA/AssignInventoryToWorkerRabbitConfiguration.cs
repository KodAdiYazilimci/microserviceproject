
using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Business.Configuration.Communication.Rabbit.AA
{
    /// <summary>
    /// Çalışana envanter ekleyecek rabbit kuyruğu için yapılandırma sınıfı
    /// </summary>
    public class AssignInventoryToWorkerRabbitConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Çalışana envanter ekleyecek rabbit kuyruğu için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public AssignInventoryToWorkerRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("AA")
                .GetSection("QueueNames")
                .GetSection("AssignInventoryToWorker").Value;
        }
    }
}
