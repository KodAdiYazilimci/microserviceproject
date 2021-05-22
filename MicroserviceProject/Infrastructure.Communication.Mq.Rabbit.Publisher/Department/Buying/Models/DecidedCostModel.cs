namespace Infrastructure.Communication.Mq.Rabbit.Publisher.Department.Buying.Models
{
    /// <summary>
    /// Envanter satın alma kararı modeli
    /// </summary>
    public class DecidedCostModel
    {
        public int InventoryRequestId { get; set; }
        public bool Approved { get; set; }
        public bool Done { get; set; }
    }
}
