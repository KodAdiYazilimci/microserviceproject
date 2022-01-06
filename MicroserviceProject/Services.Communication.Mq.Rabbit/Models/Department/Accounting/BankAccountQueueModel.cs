namespace Services.Communication.Mq.Rabbit.Department.Models.Accounting
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
