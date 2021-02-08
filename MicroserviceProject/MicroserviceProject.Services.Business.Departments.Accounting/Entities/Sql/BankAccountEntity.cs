using System.Collections.Generic;

namespace MicroserviceProject.Services.Business.Departments.Accounting.Entities.Sql
{
    public class BankAccountEntity : BaseEntity
    {
        public int WorkerId { get; set; }
        public string IBAN { get; set; }

        public virtual ICollection<SalaryPaymentEntity> SalaryPayments { get; set; }
    }
}
