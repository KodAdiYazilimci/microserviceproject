using MicroserviceProject.Services.Transaction.Types;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Services.Transaction.Models
{
    /// <summary>
    /// Tüm işlemleri geri alma modeli
    /// </summary>
    public class RollbackModel
    {
        /// <summary>
        /// İşlemin kimliği
        /// </summary>
        public string TransactionIdentity { get; set; }

        /// <summary>
        /// İşleme konu olan tip
        /// </summary>
        public TransactionType TransactionType { get; set; }

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
        public List<RollbackItemModel> RollbackItems { get; set; }
    }
}
