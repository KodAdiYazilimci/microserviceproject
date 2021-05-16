namespace Services.Business.Departments.AA.Models
{
    /// <summary>
    /// İdari işler envanterleri
    /// </summary>
    public class InventoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentStockCount { get; set; }
    }
}
