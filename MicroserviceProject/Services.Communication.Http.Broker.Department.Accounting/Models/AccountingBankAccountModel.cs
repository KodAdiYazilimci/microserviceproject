namespace Services.Communication.Http.Broker.Department.Accounting.Models
{
    /// <summary>
    /// Banka hesapları
    /// </summary>
    public class AccountingBankAccountModel
    {
        /// <summary>
        /// Hesabın IBAN numarası
        /// </summary>
        public string IBAN { get; set; }

        /// <summary>
        /// Hesabın sahibi çalışan
        /// </summary>
        public AccountingWorkerModel Worker { get; set; }
    }
}
