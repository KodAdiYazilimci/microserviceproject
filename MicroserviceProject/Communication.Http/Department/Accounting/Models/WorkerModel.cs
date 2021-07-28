using System;
using System.Collections.Generic;

namespace Communication.Http.Department.Accounting.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class WorkerModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Çalışanın departmanı
        /// </summary>
        public DepartmentModel Department { get; set; }

        /// <summary>
        /// Çalışan kişi
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// Çalışanın ünvanı
        /// </summary>
        public TitleModel Title { get; set; }

        /// <summary>
        /// Başlama tarihi
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Bitiş tarihi
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Çalışanın banka hesapları
        /// </summary>
        public List<BankAccountModel> BankAccounts { get; set; }

        /// <summary>
        /// Çalışanın IT envanterleri
        /// </summary>
        public List<InventoryModel> ITInventories { get; set; }

        /// <summary>
        /// Çalışanın idari işler envanterleri
        /// </summary>
        public List<InventoryModel> AAInventories { get; set; }
    }
}
