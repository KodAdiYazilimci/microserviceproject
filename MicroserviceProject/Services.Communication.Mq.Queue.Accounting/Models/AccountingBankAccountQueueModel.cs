using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.Accounting.Models
{
    /// <summary>
    /// Banka hesapları
    /// </summary>
    public class AccountingBankAccountQueueModel : BaseQueueModel
    {
        /// <summary>
        /// Hesabın IBAN numarası
        /// </summary>
        public string IBAN { get; set; }

        /// <summary>
        /// Hesabın sahibi çalışan
        /// </summary>
        public AccountingWorkerQueueModel Worker { get; set; }
    }
}
