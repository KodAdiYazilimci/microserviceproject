
using Microsoft.Extensions.Configuration;

using Services.Communication.Mq.Configuration;

namespace Services.Communication.Mq.Queue.IT.Configuration
{
    /// <summary>
    /// Çalışana envanter ekleyecek rabbit kuyruğu için yapılandırma sınıfı
    /// </summary>
    public class ITAssignInventoryToWorkerRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Çalışana envanter ekleyecek rabbit kuyruğu için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public ITAssignInventoryToWorkerRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName = "it.queue.inventory.assigninventorytoworker";
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
