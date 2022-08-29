
using Microsoft.Extensions.Configuration;

using Services.Communication.Mq.Configuration;

namespace Services.Communication.Mq.Queue.Storage.Configuration
{
    /// <summary>
    /// Depolama departmanına bir ürünün stoğunun artırılması için yapılandırma sınıfı
    /// </summary>
    public class IncreaseProductStockRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Depolama departmanına bir ürünün stoğunun artırılması için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public IncreaseProductStockRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName = "storage.queue.product.increaseproductstock";
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
