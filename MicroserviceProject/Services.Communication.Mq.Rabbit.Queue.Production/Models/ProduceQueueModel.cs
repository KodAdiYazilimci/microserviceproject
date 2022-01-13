namespace Services.Communication.Mq.Rabbit.Queue.Production.Models
{
    public class ProduceQueueModel : BaseQueueModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int DepartmentId { get; set; }
        public int ReferenceNumber { get; set; }
    }
}
