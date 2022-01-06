using System;

namespace Services.Communication.Mq.Rabbit.Department.Models.AA
{
    /// <summary>
    /// İdari işler envanterleri
    /// </summary>
    public class InventoryQueueModel : BaseQueueModel
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
