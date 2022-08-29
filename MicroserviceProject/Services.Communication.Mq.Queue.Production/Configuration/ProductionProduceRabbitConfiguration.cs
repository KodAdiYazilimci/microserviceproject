
using Microsoft.Extensions.Configuration;

using Services.Communication.Mq.Configuration;

namespace Services.Communication.Mq.Queue.Production.Configuration
{
    /// <summary>
    /// Üretilecek ürünlerin rabbit kuyruğu için yapılandırma sınıfı
    /// </summary>
    public class ProductionProduceRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Üretilecek ürünlerin rabbit kuyruğu için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public ProductionProduceRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName = "production.queue.product.produce";
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
