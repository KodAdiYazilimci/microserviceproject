using Services.Communication.Mq.Rabbit.Publisher;
using Services.Communication.Mq.Rabbit.Queue.Selling.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Selling.Models;

namespace Services.Communication.Mq.Rabbit.Queue.Selling.Publishers
{
    /// <summary>
    /// Üretilmesi talep edilmiş ürünlerin onay sonuçlarını kuyruğa ekler
    /// </summary>
    public class NotifyProductionRequestApprovementPublisher : BasePublisher<ProductionRequestQueueModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Üretilmesi talep edilmiş ürünlerin onay sonuçlarını kuyruğa ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public NotifyProductionRequestApprovementPublisher(
            NotifyProductionRequestApprovementRabbitConfiguration rabbitConfiguration)
            : base(rabbitConfiguration)
        {

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
