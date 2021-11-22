using System.Collections.Generic;

namespace Services.Api.Business.Departments.IT.Entities.Sql
{
    /// <summary>
    /// IT ye ait envanterler entity sınıfı
    /// </summary>
    public class InventoryEntity : BaseEntity
    {
        /// <summary>
        /// Envanterin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Şu an bulunduğu stok adedi
        /// </summary>
        public int CurrentStockCount { get; set; }

        /// <summary>
        /// Envanteri kullanan çalışanlar
        /// </summary>
        public virtual ICollection<WorkerInventoryEntity> WorkerInventories { get; set; }

        /// <summary>
        /// Envantere ait varsayılanlar bilgisi
        /// </summary>
        public virtual ICollection<InventoryDefaultsEntity> InventoryDefaults { get; set; }
    }
}
