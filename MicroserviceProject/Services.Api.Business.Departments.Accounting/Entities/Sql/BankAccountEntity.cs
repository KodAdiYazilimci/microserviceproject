using System.Collections.Generic;

namespace Services.Api.Business.Departments.Accounting.Entities.Sql
{
    public class BankAccountEntity : BaseEntity
    {
        public int WorkerId { get; set; }
        public string IBAN { get; set; }

        public virtual ICollection<SalaryPaymentEntity> SalaryPayments { get; set; }
    }
}
