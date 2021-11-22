
using Services.Communication.Mq.Rabbit.Configuration.Department.Storage;
using Services.Communication.Mq.Rabbit.Department.Models.Storage;

using System;

namespace Services.Communication.Mq.Rabbit.Publisher.Department.Storage
{
    /// <summary>
    /// Depolama departmanına bir ürünün stoğunun artırılması için kayıt açar
    /// </summary>
    public class IncreaseProductStockPublisher : BasePublisher<ProductStockQueueModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Depolama departmanına bir ürünün stoğunun artırılması için kayıt açar
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public IncreaseProductStockPublisher(
            IncreaseProductStockRabbitConfiguration rabbitConfiguration)
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
