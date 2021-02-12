using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
