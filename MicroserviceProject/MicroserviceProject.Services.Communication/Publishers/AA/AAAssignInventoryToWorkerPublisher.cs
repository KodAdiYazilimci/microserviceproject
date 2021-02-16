using MicroserviceProject.Services.Model.Department.HR;
using MicroserviceProject.Services.Communication.Configuration.Rabbit.AA;

namespace MicroserviceProject.Services.Communication.Publishers.AA
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
