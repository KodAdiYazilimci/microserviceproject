using System.Collections.Generic;

namespace Services.Business.Departments.HR.Entities.Sql
{
    /// <summary>
    /// Kişi tablosu entity sınıfı
    /// </summary>
    public class PersonEntity : BaseEntity
    {
        /// <summary>
        /// Kişinin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kişiye ait çalışan kayıtları
        /// </summary>
        public virtual ICollection<WorkerEntity> Workers { get; set; }
    }
}
