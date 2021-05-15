using System;
using System.Collections.Generic;

namespace Infrastructure.Scheduling.Departments.Finance.Models
{
    public class ExchangeModel
    {
        public DateTime Date{ get; set; }
        public string Bulten { get; set; }

        public Tarih_Date Tarih_Date { get; set; } = new Tarih_Date();
    }

    public class Tarih_Date
    {
        public List<Currency> Currencies { get; set; } = new List<Currency>();
    }

    public class Currency
    {
        public int Unit { get; set; }
        public string Isim { get; set; }
        public string CurrencyName { get; set; }
        public decimal? ForexBuying { get; set; }
        public decimal? ForexSelling { get; set; }
        public decimal? BanknoteBuying { get; set; }
        public decimal ?BanknoteSelling { get; set; }
        public decimal? CrossRateUSD { get; set; }
        public decimal? CrossRateOther { get; set; }
    }
}
