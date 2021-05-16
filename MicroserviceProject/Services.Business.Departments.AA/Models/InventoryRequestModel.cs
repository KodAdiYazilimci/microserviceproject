namespace Services.Business.Departments.AA.Models
{
    /// <summary>
    /// Envanter talebi modeli
    /// </summary>
    public class InventoryRequestModel
    {
        public int InventoryId { get; set; }
        public int Amount { get; set; }
        public bool Revoked { get; set; }
    }
}
