using MicroserviceProject.Services.Business.Configuration.Communication.Rabbit.AA;
using MicroserviceProject.Services.Business.Model.Department.HR;

namespace MicroserviceProject.Services.Business.Util.Communication.Rabbit.AA
{
    /// <summary>
    /// Çalışana envanter ekleyecek rabbit kuyruğuna yeni bir kayıt ekler
    /// </summary>
    public class AssignInventoryToWorkerPublisher : BasePublisher<WorkerModel>
    {
        /// <summary>
        /// Çalışana envanter ekleyecek rabbit kuyruğuna yeni bir kayıt ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public AssignInventoryToWorkerPublisher(
            AssignInventoryToWorkerRabbitConfiguration rabbitConfiguration)
            : base(rabbitConfiguration)
        {

        }
    }
}
