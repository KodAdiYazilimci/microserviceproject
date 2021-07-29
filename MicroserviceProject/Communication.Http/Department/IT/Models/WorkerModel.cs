using System;
using System.Collections.Generic;

namespace Communication.Http.Department.IT.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class WorkerModel
    {
        public int Id { get; set; }    

        /// <summary>
        /// Başlama tarihi
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Bitiş tarihi
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Çalışanın IT envanterleri
        /// </summary>
        public List<InventoryModel> ITInventories { get; set; }
    }
}
