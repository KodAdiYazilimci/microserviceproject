using System.Collections.Generic;

namespace Services.Api.Business.Departments.HR.Entities.Sql
{
    /// <summary>
    /// Departman tablosu entity sınıfı
    /// </summary>
    public class DepartmentEntity : BaseEntity
    {
        /// <summary>
        /// Departmanın adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Departmanın çalışanları
        /// </summary>
        public virtual ICollection<WorkerEntity> Workers { get; set; }
    }
}
