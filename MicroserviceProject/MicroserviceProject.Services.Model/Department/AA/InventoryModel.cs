using System;

namespace MicroserviceProject.Services.Model.Department.AA
{
    /// <summary>
    /// İdari işler envanterleri
    /// </summary>
    public class InventoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
