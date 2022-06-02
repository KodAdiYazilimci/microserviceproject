using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.Storage.Models
{
    public class ProductStockQueueModel : BaseQueueModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
