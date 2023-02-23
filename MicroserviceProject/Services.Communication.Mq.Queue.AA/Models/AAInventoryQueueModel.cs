using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.AA.Models
{
    /// <summary>
    /// İdari işler envanterleri
    /// </summary>
    public class AAInventoryQueueModel : BaseQueueModel
    {
        public int InventoryId { get; set; }
        public int WorkerId { get; set; }
        public int Amount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
