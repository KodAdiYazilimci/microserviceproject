using MicroserviceProject.Services.Communication.Configuration.Rabbit.AA;
using MicroserviceProject.Services.Model.Department.Buying;

using System;

namespace MicroserviceProject.Services.Communication.Publishers.IT
{
    /// <summary>
    /// İdari işler departmanına satın alımla ilgili olumlu veya olumsuz dönüş verisini rabbit kuyruğuna ekler
    /// </summary>
    public class AAInformInventoryRequestPublisher : BasePublisher<InventoryRequestModel>, IDisposable
    {
        /// <summary>
        /// İdari işler departmanına satın alımla ilgili olumlu veya olumsuz dönüş verisini rabbit kuyruğuna ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public AAInformInventoryRequestPublisher(
            AAInformInventoryRequestRabbitConfiguration rabbitConfiguration)
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
                if (!Disposed)
                {
                    Disposed = true;
                }
            }
        }
    }
}
