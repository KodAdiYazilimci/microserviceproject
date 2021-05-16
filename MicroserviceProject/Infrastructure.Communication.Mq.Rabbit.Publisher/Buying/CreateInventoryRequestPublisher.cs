using Infrastructure.Communication.Mq.Rabbit.Configuration.Buying;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Buying.Models;

using System;

namespace Infrastructure.Communication.Mq.Rabbit.Publisher.Buying
{
    /// <summary>
    /// Satınalma departmanına alınması istenilen envanter talepleri için kayıt açar
    /// </summary>
    public class CreateInventoryRequestPublisher : BasePublisher<InventoryRequestModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Satınalma departmanına alınması istenilen envanter talepleri için kayıt açar
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public CreateInventoryRequestPublisher(
            CreateInventoryRequestRabbitConfiguration rabbitConfiguration)
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
