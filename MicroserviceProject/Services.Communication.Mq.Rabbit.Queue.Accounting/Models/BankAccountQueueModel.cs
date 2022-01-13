namespace Services.Communication.Mq.Rabbit.Queue.Accounting.Models
{
    /// <summary>
    /// Banka hesapları
    /// </summary>
    public class BankAccountQueueModel : BaseQueueModel
    {
        /// <summary>
        /// Hesabın IBAN numarası
        /// </summary>
        public string IBAN { get; set; }

        /// <summary>
        /// Hesabın sahibi çalışan
        /// </summary>
        public WorkerQueueModel Worker { get; set; }
    }
}
