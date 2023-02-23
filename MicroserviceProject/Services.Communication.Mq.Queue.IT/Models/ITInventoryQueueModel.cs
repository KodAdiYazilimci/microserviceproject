
using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.IT.Models
{
    /// <summary>
    /// IT envanterleri
    /// </summary>
    public class ITInventoryQueueModel : BaseQueueModel
    {
        public int InventoryId { get; set; }
        public int WorkerId { get; set; }
        public int Amount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
