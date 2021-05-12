using System.Collections.Generic;

namespace MicroserviceProject.Services.Business.Departments.Accounting.Entities.Sql
{
    public class CurrencyEntity : BaseEntity
    {
        public string Name { get; set; }
        public string ShortName { get; set; }

        public virtual ICollection<SalaryPaymentEntity> SalaryPayments { get; set; }
    }
}
