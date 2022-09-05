
using Microsoft.Extensions.Configuration;

using Services.Communication.Mq.Configuration;

namespace Services.Communication.Mq.Queue.Buying.Configuration
{
    /// <summary>
    /// Satınalma departmanına alınması istenilen envanter talepleri için yapılandırma sınıfı
    /// </summary>
    public class CreateInventoryRequestRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Satınalma departmanına alınması istenilen envanter talepleri için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public CreateInventoryRequestRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName = "buying.queue.request.createinventoryrequest";
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
