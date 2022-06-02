using Services.Communication.Mq.Queue.Storage.Configuration;
using Services.Communication.Mq.Queue.Storage.Models;
using Services.Communication.Mq.Rabbit.Publisher;

namespace Services.Communication.Mq.Queue.Storage.Rabbit.Publishers
{
    /// <summary>
    /// Depolama departmanına bir ürünün stoğunun düşürülmesi için kayıt açar
    /// </summary>
    public class DescendProductStockPublisher : BasePublisher<ProductStockQueueModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Depolama departmanına bir ürünün stoğunun düşürülmesi için kayıt açar
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public DescendProductStockPublisher(
            DescendProductStockRabbitConfiguration rabbitConfiguration)
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
