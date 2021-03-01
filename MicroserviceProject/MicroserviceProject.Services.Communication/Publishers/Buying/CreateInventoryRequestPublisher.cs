using MicroserviceProject.Services.Communication.Configuration.Rabbit.Buying;
using MicroserviceProject.Services.Model.Department.Buying;

using System;

namespace MicroserviceProject.Services.Communication.Publishers.Buying
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

                }

                disposed = true;

                Dispose();
            }
        }
    }
}
