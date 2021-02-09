
using MicroserviceProject.Presentation.UI.Business.Model.Department.Accounting;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Presentation.UI.Business.Model.Department.HR
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
        /// Yöneticileri
        /// </summary>
        public List<WorkerModel> Managers { get; set; }

        /// <summary>
        /// Ast çalışanları
        /// </summary>
        public List<WorkerModel> Workers { get; set; }

        /// <summary>
        /// Çalışanın banka hesapları
        /// </summary>
        public List<BankAccountModel> BankAccounts { get; set; }

        public override string ToString()
        {
            return $"{this.Title} {this.Person.Name}({this.Department.Name})";
        }
    }
}
