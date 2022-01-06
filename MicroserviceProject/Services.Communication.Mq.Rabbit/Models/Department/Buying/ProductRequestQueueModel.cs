namespace Services.Communication.Mq.Rabbit.Department.Models.Buying
{
    public class ProductRequestQueueModel : BaseQueueModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int ReferenceNumber { get; set; }
    }
}
