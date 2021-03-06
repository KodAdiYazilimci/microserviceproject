using MicroserviceProject.Services.Model.Department.HR;
using MicroserviceProject.Services.Communication.Configuration.Rabbit.IT;
using System;

namespace MicroserviceProject.Services.Communication.Publishers.IT
{
    /// <summary>
    /// Çalışana envanter ekleyecek rabbit kuyruğuna yeni bir kayıt ekler
    /// </summary>
    public class ITAssignInventoryToWorkerPublisher : BasePublisher<WorkerModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Çalışana envanter ekleyecek rabbit kuyruğuna yeni bir kayıt ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public ITAssignInventoryToWorkerPublisher(
            ITAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration)
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
