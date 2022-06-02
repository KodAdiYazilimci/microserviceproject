using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.Finance.Models
{
    /// <summary>
    /// Envanter satın alım kararı modeli
    /// </summary>
    public class DecidedCostQueueModel : BaseQueueModel
    {
        public int InventoryRequestId { get; set; }
    }
}
