using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.AA.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class WorkerQueueModel : BaseQueueModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Çalışanın envanterleri
        /// </summary>
        public List<InventoryQueueModel> Inventories { get; set; }
    }
}
