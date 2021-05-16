using System;
using System.Collections.Generic;

namespace Infrastructure.Communication.Mq.Rabbit.Publisher.IT.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class WorkerModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Çalışanın envanterleri
        /// </summary>
        public List<InventoryModel> Inventories { get; set; }
    }
}
