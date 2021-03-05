using System;

namespace MicroserviceProject.Presentation.UI.WindowsForm.Business.Model.Department.IT
{
    /// <summary>
    /// IT envanterleri
    /// </summary>
    public class InventoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentStockCount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public override string ToString()
        {
            return $"{this.Name} ({CurrentStockCount} Mevcut)";
        }
    }
}
