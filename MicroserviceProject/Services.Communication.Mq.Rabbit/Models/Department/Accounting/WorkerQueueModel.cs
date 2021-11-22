using System.Collections.Generic;

namespace Services.Communication.Mq.Rabbit.Department.Models.Accounting
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class WorkerQueueModel
    {
        public int Id { get; set; }

        public List<BankAccountQueueModel> BankAccounts { get; set; }
    }
}
