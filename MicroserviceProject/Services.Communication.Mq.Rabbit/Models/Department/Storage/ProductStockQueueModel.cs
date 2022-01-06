namespace Services.Communication.Mq.Rabbit.Department.Models.Storage
{
    public class ProductStockQueueModel : BaseQueueModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
