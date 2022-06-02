using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.Production.Models
{
    public class ProduceQueueModel : BaseQueueModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int DepartmentId { get; set; }
        public int ReferenceNumber { get; set; }
    }
}
