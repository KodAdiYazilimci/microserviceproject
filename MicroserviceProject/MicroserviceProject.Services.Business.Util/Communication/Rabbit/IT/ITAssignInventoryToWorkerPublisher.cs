using MicroserviceProject.Services.Business.Model.Department.HR;
using MicroserviceProject.Services.Configuration.Communication.Rabbit.IT;

namespace MicroserviceProject.Services.Business.Util.Communication.Rabbit.IT
{
    /// <summary>
    /// Çalışana envanter ekleyecek rabbit kuyruğuna yeni bir kayıt ekler
    /// </summary>
    public class ITAssignInventoryToWorkerPublisher : BasePublisher<WorkerModel>
    {
        /// <summary>
        /// Çalışana envanter ekleyecek rabbit kuyruğuna yeni bir kayıt ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public ITAssignInventoryToWorkerPublisher(
            ITAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration)
            : base(rabbitConfiguration)
        {

        }
    }
}
