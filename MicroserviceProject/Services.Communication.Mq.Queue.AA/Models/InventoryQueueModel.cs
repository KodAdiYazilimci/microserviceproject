using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.AA.Models
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
