using System.Collections.Generic;

namespace Services.Communication.Mq.Rabbit.Department.Models.AA
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class WorkerQueueModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Çalışanın envanterleri
        /// </summary>
        public List<InventoryQueueModel> Inventories { get; set; }
    }
}
