namespace Services.Communication.Mq.Rabbit.Queue.Finance.Models
{
    /// <summary>
    /// Envanter satın alım kararı modeli
    /// </summary>
    public class DecidedCostQueueModel : BaseQueueModel
    {
        public int InventoryRequestId { get; set; }
    }
}
