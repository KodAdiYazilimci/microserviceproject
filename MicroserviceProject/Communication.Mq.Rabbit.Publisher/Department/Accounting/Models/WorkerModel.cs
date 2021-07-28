using System.Collections.Generic;

namespace Communication.Mq.Rabbit.Publisher.Department.Accounting.Models
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
