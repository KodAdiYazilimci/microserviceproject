using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.Accounting.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class AccountingWorkerQueueModel : BaseQueueModel
    {
        public int Id { get; set; }

        public List<AccountingBankAccountQueueModel> BankAccounts { get; set; }
    }
}
