namespace Services.Model.Department.Finance
{
    /// <summary>
    /// Envanter alım kararı modeli
    /// </summary>
    public class DecidedCostModel
    {
        public int Id { get; set; }
        public int InventoryRequestId { get; set; }
        public bool Approved { get; set; }
    }
}
