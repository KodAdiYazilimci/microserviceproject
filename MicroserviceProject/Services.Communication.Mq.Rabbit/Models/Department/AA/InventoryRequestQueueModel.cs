namespace Services.Communication.Mq.Rabbit.Department.Models.AA
{
    /// <summary>
    /// Envanter alım talebi modeli
    /// </summary>
    public class InventoryRequestQueueModel
    {
        public int InventoryId { get; set; }
        public int Amount { get; set; }
        public bool Revoked { get; set; }
        public bool Done { get; set; }
    }
}
