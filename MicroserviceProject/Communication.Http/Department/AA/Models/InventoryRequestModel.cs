namespace Communication.Http.Department.AA.Models
{
    /// <summary>
    /// Envanter alım talebi modeli
    /// </summary>
    public class InventoryRequestModel
    {
        public int InventoryId { get; set; }
        public int Amount { get; set; }
        public bool Revoked { get; set; }
        public bool Done { get; set; }
    }
}
