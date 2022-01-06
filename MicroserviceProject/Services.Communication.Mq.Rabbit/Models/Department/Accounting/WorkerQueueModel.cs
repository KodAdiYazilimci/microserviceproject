using System.Collections.Generic;

namespace Services.Communication.Mq.Rabbit.Department.Models.Accounting
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class WorkerQueueModel : BaseQueueModel
    {
        public int Id { get; set; }

        public List<BankAccountQueueModel> BankAccounts { get; set; }
    }
}
