namespace Services.Communication.Mq.Rabbit.Department.Models.Buying
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
