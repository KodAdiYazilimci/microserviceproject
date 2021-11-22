using System;

namespace Services.Api.Business.Departments.IT.Entities.Sql
{
    /// <summary>
    /// AA ye ait çalışanlara verilecek stoğu olmayan envanterler entity sınıfı
    /// </summary>
    public class PendingWorkerInventoryEntity : BaseEntity
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
        /// Beklenen stok adedi
        /// </summary>
        public int StockCount { get; set; }

        /// <summary>
        /// Beklentinin tamamlanıp tamamlanmadığı bilgisi
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Kullanılan envanter
        /// </summary>
        public virtual InventoryEntity Inventory { get; set; }
    }
}
