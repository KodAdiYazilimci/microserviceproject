using System;

namespace Services.Communication.Http.Broker.Department.Accounting.Models
{
    /// <summary>
    /// Maaş ödemeleri
    /// </summary>
    public class AccountingSalaryPaymentModel
    {
        /// <summary>
        /// Ödenen miktar
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Ödeme yapılan tarih
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Ödeme yapılan çalışan hesabı
        /// </summary>
        public AccountingBankAccountModel BankAccount { get; set; }

        /// <summary>
        /// Para birimi
        /// </summary>
        public AccountingCurrencyModel Currency { get; set; }
    }
}
