using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.Buying.Models
{
    /// <summary>
    /// Envanter satın alma kararı modeli
    /// </summary>
    public class DecidedCostQueueModel : BaseQueueModel
    {
        public int InventoryRequestId { get; set; }
        public bool Approved { get; set; }
        public bool Done { get; set; }
    }
}
