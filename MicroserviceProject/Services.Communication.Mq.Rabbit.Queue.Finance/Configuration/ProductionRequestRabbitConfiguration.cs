
using Microsoft.Extensions.Configuration;

using Services.Communication.Mq.Rabbit.Configuration;

namespace Services.Communication.Mq.Rabbit.Queue.Finance.Configuration
{
    /// <summary>
    /// Finans departmanına üretilmesi istenilen ürünler için talep açacak yapılandırma sınıfı
    /// </summary>
    public class ProductionRequestRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Finans departmanına üretilmesi istenilen ürünler için talep açacak yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public ProductionRequestRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("Finance")
                .GetSection("QueueNames")["ProductionRequest"];
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;
                }
            }
        }
    }
}
