namespace Services.Communication.Mq.Rabbit.Queue.Buying.Models
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
