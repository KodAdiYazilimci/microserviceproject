using System;

namespace Services.Communication.Mq.Rabbit.Department.Models.IT
{
    /// <summary>
    /// IT envanterleri
    /// </summary>
    public class InventoryQueueModel : BaseQueueModel
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
