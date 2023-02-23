using Services.Communication.Mq.Queue.IT.Configuration;
using Services.Communication.Mq.Queue.IT.Models;
using Services.Communication.Mq.Rabbit.Publisher;

namespace Services.Communication.Mq.Queue.IT.Rabbit.Publishers
{
    /// <summary>
    /// Bilgi teknolojileri departmanına satın alımla ilgili olumlu veya olumsuz dönüş verisini rabbit kuyruğuna ekler
    /// </summary>
    public class ITInformInventoryRequestPublisher : BasePublisher<ITInventoryRequestQueueModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Bilgi teknolojileri departmanına satın alımla ilgili olumlu veya olumsuz dönüş verisini rabbit kuyruğuna ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public ITInformInventoryRequestPublisher(
            InformInventoryRequestRabbitConfiguration rabbitConfiguration)
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
