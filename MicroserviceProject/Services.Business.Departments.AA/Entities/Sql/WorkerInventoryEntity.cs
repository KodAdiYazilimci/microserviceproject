using System;

namespace Services.Business.Departments.AA.Entities.Sql
{
    /// <summary>
    /// AA ye ait envanterleri kullanan çalışanlar entity sınıfı
    /// </summary>
    public class WorkerInventoryEntity : BaseEntity
    {
        /// <summary>
        /// Envanterin Id si
        /// </summary>
        public int InventoryId { get; set; }

        /// <summary>
        /// Çalışanın Id si
        /// </summary>
        public int WorkerId { get; set; }

        /// <summary>
        /// Envanterin kullanılmaya başlandığı tarih
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Envanterin son kullanılma tarihi
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Kullanılan envanter
        /// </summary>
        public virtual InventoryEntity Inventory { get; set; }
    }
}
