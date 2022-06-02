using Services.Communication.Mq.Queue.Buying.Configuration;
using Services.Communication.Mq.Queue.Buying.Models;
using Services.Communication.Mq.Rabbit.Publisher;

namespace Services.Communication.Mq.Queue.Buying.Rabbit.Publishers
{
    /// <summary>
    /// Satın alınması istenilen envanterlere ait kararları rabbit kuyruğuna ekler
    /// </summary>
    public class NotifyCostApprovementPublisher : BasePublisher<DecidedCostQueueModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Satın alınması istenilen envanterlere ait kararları rabbit kuyruğuna ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public NotifyCostApprovementPublisher(
            NotifyCostApprovementRabbitConfiguration rabbitConfiguration)
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
