namespace Services.Communication.Mq.Rabbit.Queue.Buying.Models
{
    public class ProductRequestQueueModel : BaseQueueModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int ReferenceNumber { get; set; }
    }
}
