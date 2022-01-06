namespace Services.Communication.Mq.Rabbit.Department.Models.Production
{
    public class ProduceQueueModel : BaseQueueModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int DepartmentId { get; set; }
        public int ReferenceNumber { get; set; }
    }
}
