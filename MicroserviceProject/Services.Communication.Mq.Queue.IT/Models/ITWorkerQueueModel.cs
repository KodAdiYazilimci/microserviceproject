
using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.IT.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class ITWorkerQueueModel : BaseQueueModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Çalışanın envanterleri
        /// </summary>
        public List<ITInventoryQueueModel> Inventories { get; set; }
    }
}
