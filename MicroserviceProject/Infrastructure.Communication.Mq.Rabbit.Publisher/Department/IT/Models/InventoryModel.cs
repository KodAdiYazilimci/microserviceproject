using System;

namespace Infrastructure.Communication.Mq.Rabbit.Publisher.Department.IT.Models
{
    /// <summary>
    /// IT envanterleri
    /// </summary>
    public class InventoryModel
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
