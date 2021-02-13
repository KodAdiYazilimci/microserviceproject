using MicroserviceProject.Services.Business.Model.Department.HR;
using MicroserviceProject.Services.Configuration.Communication.Rabbit.AA;

namespace MicroserviceProject.Services.Business.Util.Communication.Rabbit.AA
{
    /// <summary>
    /// Çalışana envanter ekleyecek rabbit kuyruğuna yeni bir kayıt ekler
    /// </summary>
    public class AAAssignInventoryToWorkerPublisher : BasePublisher<WorkerModel>
    {
        /// <summary>
        /// Çalışana envanter ekleyecek rabbit kuyruğuna yeni bir kayıt ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public AAAssignInventoryToWorkerPublisher(
            AAAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration)
            : base(rabbitConfiguration)
        {

        }
    }
}
