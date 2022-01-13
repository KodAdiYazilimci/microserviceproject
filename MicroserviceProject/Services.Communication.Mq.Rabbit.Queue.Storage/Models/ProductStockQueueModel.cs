namespace Services.Communication.Mq.Rabbit.Queue.Storage.Models
{
    public class ProductStockQueueModel : BaseQueueModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
