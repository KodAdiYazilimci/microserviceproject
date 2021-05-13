using System;

namespace Services.Business.Departments.HR.Entities.Sql
{
    /// <summary>
    /// Çalışan ast-üst ilişkileri tablosu entity sınıfı
    /// </summary>
    public class WorkerRelationEntity : BaseEntity
    {
        /// <summary>
        /// Çalışanın Id si
        /// </summary>
        public int WorkerId { get; set; }

        /// <summary>
        /// Yöneticisinin Id si
        /// </summary>
        public int ManagerId { get; set; }

        /// <summary>
        /// Başlangıç tarihi
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Sona eriş tarihi
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// İlişkinin ast çalışanı
        /// </summary>
        public virtual WorkerEntity Worker { get; set; }

        /// <summary>
        /// İlişkinin üstü
        /// </summary>
        public virtual WorkerEntity Manager { get; set; }
    }
}
