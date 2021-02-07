using System;
using System.Collections.Generic;

namespace MicroserviceProject.Services.Business.Departments.HR.Entities.Sql
{
    /// <summary>
    /// Çalışanlar tablosu entity sınıfı
    /// </summary>
    public class WorkerEntity : BaseEntity
    {
        /// <summary>
        /// Çalışanın adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Başlama tarihi
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Bitiş tarihi
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Çalışanın departman Id si
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Çalışanın kişi Id si
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Çalışanın ünvan Id si
        /// </summary>
        public int TitleId { get; set; }

        /// <summary>
        /// Çalışanın departmanı
        /// </summary>
        public virtual DepartmentEntity Department { get; set; }

        /// <summary>
        /// Çalışan kişi
        /// </summary>
        public virtual PersonEntity Person { get; set; }

        /// <summary>
        /// Çalışanın ünvanı
        /// </summary>
        public virtual TitleEntity Title { get; set; }

        /// <summary>
        /// Ast çalışanları
        /// </summary>
        public virtual ICollection<WorkerRelationEntity> WorkerRelations { get; set; }
    }
}
