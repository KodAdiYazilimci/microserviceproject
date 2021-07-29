﻿using System;

namespace Communication.Http.Department.Accounting.Models
{
    /// <summary>
    /// Maaş ödemeleri
    /// </summary>
    public class SalaryPaymentModel
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
        public BankAccountModel BankAccount { get; set; }

        /// <summary>
        /// Para birimi
        /// </summary>
        public CurrencyModel Currency { get; set; }
    }
}
