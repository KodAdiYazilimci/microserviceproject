using System;
using System.Collections.Generic;

namespace Services.Api.Business.Departments.CR.Entities.EntityFramework
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
