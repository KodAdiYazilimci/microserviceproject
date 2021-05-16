namespace Services.Business.Departments.Buying.Models
{
    /// <summary>
    /// Envanter satın alım kararı modeli
    /// </summary>
    public class DecidedCostModel
    {
        public int InventoryRequestId { get; set; }
        public bool Approved { get; set; }
    }
}
