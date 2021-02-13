using System.Collections.Generic;

namespace MicroserviceProject.Services.Business.Departments.AA.Entities.Sql
{
    /// <summary>
    /// AA ye ait envanterler entity sınıfı
    /// </summary>
    public class InventoryEntity : BaseEntity
    {
        /// <summary>
        /// Envanterin adı
        /// </summary>
        public string Name { get; set; }

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
