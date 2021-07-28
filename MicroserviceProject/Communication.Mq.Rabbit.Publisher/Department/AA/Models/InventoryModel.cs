using System;

namespace Communication.Mq.Rabbit.Publisher.Department.AA.Models
{
    /// <summary>
    /// İdari işler envanterleri
    /// </summary>
    public class InventoryModel
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
