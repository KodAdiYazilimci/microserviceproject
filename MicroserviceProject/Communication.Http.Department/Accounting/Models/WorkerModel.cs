using System.Collections.Generic;

namespace Communication.Http.Department.Accounting.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class WorkerModel
    {
        public int Id { get; set; }
        public List<BankAccountModel> BankAccounts { get; set; }
    }
}
