using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.IT.Models
{
    /// <summary>
    /// Envanter talep modeli
    /// </summary>
    public class ITInventoryRequestQueueModel : BaseQueueModel
    {
        public int InventoryId { get; set; }
        public int Amount { get; set; }
        public bool Revoked { get; set; }
        public bool Done { get; set; }
    }
}
