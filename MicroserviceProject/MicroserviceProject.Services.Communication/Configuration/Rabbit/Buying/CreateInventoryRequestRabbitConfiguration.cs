
using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Communication.Configuration.Rabbit.Buying
{
    /// <summary>
    /// Satınalma departmanına alınması istenilen envanter talepleri için yapılandırma sınıfı
    /// </summary>
    public class CreateInventoryRequestRabbitConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Satınalma departmanına alınması istenilen envanter talepleri için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public CreateInventoryRequestRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("Buying")
                .GetSection("QueueNames")
                .GetSection("CreateInventoryRequest").Value;
        }
    }
}
