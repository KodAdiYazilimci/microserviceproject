
using Services.Communication.Mq.Rabbit.Configuration.Department.Buying;
using Services.Communication.Mq.Rabbit.Department.Models.Buying;

using System;

namespace Services.Communication.Mq.Rabbit.Publisher.Department.Buying
{
    /// <summary>
    /// Satınalma departmanına alınması istenilen ürün talepleri için kayıt açar
    /// </summary>
    public class CreateProductRequestPublisher : BasePublisher<ProductRequestQueueModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Satınalma departmanına alınması istenilen ürün talepleri için kayıt açar
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public CreateProductRequestPublisher(
            CreateProductRequestRabbitConfiguration rabbitConfiguration)
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
