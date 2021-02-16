using MicroserviceProject.Services.Model.Department.HR;

namespace MicroserviceProject.Services.Model.Department.Accounting
{
    /// <summary>
    /// Banka hesapları
    /// </summary>
    public class BankAccountModel
    {
        /// <summary>
        /// Hesabın IBAN numarası
        /// </summary>
        public string IBAN { get; set; }

        /// <summary>
        /// Hesap sahibi çalışan
        /// </summary>
        public WorkerModel Worker { get; set; }
    }
}
