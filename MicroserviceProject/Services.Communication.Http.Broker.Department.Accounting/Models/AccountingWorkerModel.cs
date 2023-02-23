using System.Collections.Generic;

namespace Services.Communication.Http.Broker.Department.Accounting.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class AccountingWorkerModel
    {
        public int Id { get; set; }
        public List<AccountingBankAccountModel> BankAccounts { get; set; }
    }
}
