using System;

namespace MicroserviceProject.Services.Business.Departments.Accounting.Entities.Sql
{
    public class SalaryPaymentEntity : BaseEntity
    {
        public int BankAccountId { get; set; }
        public int CurrencyId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public virtual BankAccountEntity BankAccount { get; set; }
        public virtual CurrencyEntity Currency { get; set; }
    }
}
