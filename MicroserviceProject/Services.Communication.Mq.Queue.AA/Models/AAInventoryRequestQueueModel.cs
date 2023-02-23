using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.AA.Models
{
    /// <summary>
    /// Envanter alım talebi modeli
    /// </summary>
    public class AAInventoryRequestQueueModel : BaseQueueModel
    {
        public int InventoryId { get; set; }
        public int Amount { get; set; }
        public bool Revoked { get; set; }
        public bool Done { get; set; }
    }
}
