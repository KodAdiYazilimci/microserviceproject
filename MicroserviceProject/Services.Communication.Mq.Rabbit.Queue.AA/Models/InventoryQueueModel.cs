using System;

namespace Services.Communication.Mq.Rabbit.Queue.AA.Models
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
