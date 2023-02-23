using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.AA.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class AAWorkerQueueModel : BaseQueueModel
    {
        /// <summary>
        /// Çalışanın envanterleri
        /// </summary>
        public List<AAInventoryQueueModel> Inventories { get; set; }
    }
}
