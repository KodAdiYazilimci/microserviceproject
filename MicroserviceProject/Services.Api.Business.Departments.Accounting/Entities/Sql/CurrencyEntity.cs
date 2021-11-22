using System.Collections.Generic;

namespace Services.Api.Business.Departments.Accounting.Entities.Sql
{
    public class CurrencyEntity : BaseEntity
    {
        public string Name { get; set; }
        public string ShortName { get; set; }

        public virtual ICollection<SalaryPaymentEntity> SalaryPayments { get; set; }
    }
}
