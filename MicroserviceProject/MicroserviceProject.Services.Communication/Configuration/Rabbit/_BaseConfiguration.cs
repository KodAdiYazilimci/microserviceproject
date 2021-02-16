using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Communication.Configuration.Rabbit
{
    /// <summary>
    /// Rabbit kuyrukları için temel yapılandırma sınıfı
    /// </summary>
    public class BaseConfiguration : IRabbitConfiguration
    {
        /// <summary>
        /// Rabbit kuyrukları için temel yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public BaseConfiguration(IConfiguration configuration)
        {
            Host =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("DefaultHost").Value;

            UserName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("DefaultUserName").Value;

            Password =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("DefaultPassword").Value;
        }

        /// <summary>
        /// Rabbit sunucusunun adı
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Rabbit sunucusunun kullanıcı adı
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Rabbit sunucusunun parolası
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Rabbit sunucusunun kuyruk adı
        /// </summary>
        public string QueueName { get; set; }
    }
}
