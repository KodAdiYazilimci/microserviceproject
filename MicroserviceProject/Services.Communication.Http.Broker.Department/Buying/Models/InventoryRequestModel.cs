namespace Services.Communication.Http.Broker.Department.Buying.Models
{
    /// <summary>
    /// Envanter alım talebi modeli
    /// </summary>
    public class InventoryRequestModel
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int DepartmentId { get; set; }
        public int Amount { get; set; }

        public InventoryModel AAInventory { get; set; }
        public InventoryModel ITInventory { get; set; }
    }
}
