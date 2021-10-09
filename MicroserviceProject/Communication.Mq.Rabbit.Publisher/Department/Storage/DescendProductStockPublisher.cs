using Communication.Mq.Rabbit.Publisher.Department.Storage.Models;

using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Storage;

using System;

namespace Communication.Mq.Rabbit.Publisher.Department.Storage
{
    /// <summary>
    /// Depolama departmanına bir ürünün stoğunun düşürülmesi için kayıt açar
    /// </summary>
    public class DescendProductStockPublisher : BasePublisher<ProductStockModel>, IDisposable
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
