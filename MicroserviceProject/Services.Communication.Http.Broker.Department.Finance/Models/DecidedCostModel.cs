namespace Services.Communication.Http.Broker.Department.Finance.Models
{
    /// <summary>
    /// Envanter satın alım kararı modeli
    /// </summary>
    public class DecidedCostModel
    {
        public int Id { get; set; }
        public int InventoryRequestId { get; set; }
        public bool Approved { get; set; }
    }
}
