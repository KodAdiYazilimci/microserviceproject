using Services.Communication.Mq.Rabbit.Publisher;
using Services.Communication.Mq.Rabbit.Queue.Production.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Production.Models;

namespace Services.Communication.Mq.Rabbit.Queue.Production.Publishers
{
    /// <summary>
    /// Üretilecek ürünlerin rabbit kuyruğuna yeni bir kayıt ekler
    /// </summary>
    public class ProductionProducePublisher : BasePublisher<ProduceQueueModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Üretilecek ürünlerin rabbit kuyruğuna yeni bir kayıt ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public ProductionProducePublisher(
            ProductionProduceRabbitConfiguration rabbitConfiguration)
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
