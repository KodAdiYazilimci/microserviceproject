using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.Buying.Models
{
    /// <summary>
    /// Envanter satın alım talebi modeli
    /// </summary>
    public class InventoryRequestQueueModel : BaseQueueModel
    {
        public int InventoryId { get; set; }
        public int DepartmentId { get; set; }
        public int Amount { get; set; }
    }
}
