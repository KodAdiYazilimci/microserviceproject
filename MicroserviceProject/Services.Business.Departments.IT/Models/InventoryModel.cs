using System;

namespace Services.Business.Departments.IT.Models
{
    /// <summary>
    /// IT envanterleri modeli
    /// </summary>
    public class InventoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentStockCount { get; set; }
    }
}
