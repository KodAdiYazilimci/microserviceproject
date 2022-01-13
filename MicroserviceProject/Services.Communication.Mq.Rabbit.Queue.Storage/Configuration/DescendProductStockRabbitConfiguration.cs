
using Microsoft.Extensions.Configuration;

using Services.Communication.Mq.Rabbit.Configuration;

namespace Services.Communication.Mq.Rabbit.Queue.Storage.Configuration
{
    /// <summary>
    /// Depolama departmanına bir ürünün stoğunun düşürülmesi için yapılandırma sınıfı
    /// </summary>
    public class DescendProductStockRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Depolama departmanına bir ürünün stoğunun düşürülmesi için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public DescendProductStockRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("Storage")
                .GetSection("QueueNames")["DescendProductStock"];
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
