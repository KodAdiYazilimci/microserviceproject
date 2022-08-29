
using Microsoft.Extensions.Configuration;

using Services.Communication.Mq.Configuration;

namespace Services.Communication.Mq.Queue.Authorization.Configuration
{
    /// <summary>
    /// Oturuma ait tokenın artık geçersiz olduğunu bildiren kuyruğun yapılandırma sınıfı
    /// </summary>
    public class InformInvalidTokenRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Oturuma ait tokenın artık geçersiz olduğunu bildiren kuyruğun yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public InformInvalidTokenRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName = "authorization.queue.identity.informinvalidtoken";
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
