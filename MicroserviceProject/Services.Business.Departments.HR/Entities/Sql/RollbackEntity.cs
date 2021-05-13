using System;
using System.Collections.Generic;

namespace Services.Business.Departments.HR.Entities.Sql
{
    /// <summary>
    /// Tüm işlemleri geri alma entity sınıfı
    /// </summary>
    public class RollbackEntity : BaseEntity
    {
        /// <summary>
        /// İşlemin kimliği
        /// </summary>
        public string TransactionIdentity { get; set; }

        /// <summary>
        /// İşleme konu olan tip
        /// </summary>
        public int TransactionType { get; set; }

        /// <summary>
        /// İşlemin gerçekleştiği tarih
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// İşlemin geri alınıp alınmadığı bilgisi
        /// </summary>
        public bool IsRolledback { get; set; }

        /// <summary>
        /// İşleme ait öğeler
        /// </summary>
        public virtual ICollection<RollbackItemEntity> RollbackItems { get; set; }
    }
}
