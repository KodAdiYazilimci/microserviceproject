
using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Configuration.Communication.Rabbit.Accounting
{
    /// <summary>
    /// Çalışana maaş hesabı açan rabbit kuyruğu için yapılandırma sınıfı
    /// </summary>
    public class CreateBankAccountRabbitConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Çalışana maaş hesabı açan rabbit kuyruğu için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public CreateBankAccountRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("Accounting")
                .GetSection("QueueNames")
                .GetSection("CreateBankAccount").Value;
        }
    }
}
