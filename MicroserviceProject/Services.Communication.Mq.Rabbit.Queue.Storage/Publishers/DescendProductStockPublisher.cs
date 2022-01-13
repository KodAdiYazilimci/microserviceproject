using Services.Communication.Mq.Rabbit.Publisher;
using Services.Communication.Mq.Rabbit.Queue.Storage.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Storage.Models;

namespace Services.Communication.Mq.Rabbit.Queue.Storage.Publishers
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
