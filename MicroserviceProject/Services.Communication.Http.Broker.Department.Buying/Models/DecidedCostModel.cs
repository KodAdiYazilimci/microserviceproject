namespace Services.Communication.Http.Broker.Department.Buying.Models
{
    /// <summary>
    /// Envanter satın alım kararı modeli
    /// </summary>
    public class DecidedCostModel
    {
        public int InventoryRequestId { get; set; }
        public bool Approved { get; set; }
        public bool Done { get; set; }
    }
}
