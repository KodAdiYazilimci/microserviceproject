using MicroserviceProject.Infrastructure.Communication.Model.Department.HR;

namespace MicroserviceProject.Infrastructure.Communication.Model.Department.Accounting
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
