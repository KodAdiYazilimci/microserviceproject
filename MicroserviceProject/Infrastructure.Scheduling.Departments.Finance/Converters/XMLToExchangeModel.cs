using Infrastructure.Scheduling.Departments.Finance.Models;

using System;
using System.Xml;

namespace Infrastructure.Scheduling.Departments.Finance.Converters
{
    /// <summary>
    /// XML metnini Exchange modele dönüştürür
    /// </summary>
    public static class XMLToExchangeModel
    {
        /// <summary>
        /// XML metnini Exchange modele dönüştürür
        /// </summary>
        /// <param name="text">XML metni</param>
        /// <returns></returns>
        public static ExchangeModel ConvertToExchangeModel(this string text)
        {
            ExchangeModel exchangeModel = new ExchangeModel();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(text);

            bool foundTarihDateNode = false;

            foreach (XmlNode tarihDateNode in xmlDocument.ChildNodes)
            {
                if (tarihDateNode.Name == "Tarih_Date")
                {
                    foundTarihDateNode = true;

                    foreach (XmlAttribute attribute in tarihDateNode.Attributes)
                    {
                        switch (attribute.Name)
                        {
                            case "Tarih": exchangeModel.Date = Convert.ToDateTime(attribute.Value); break;
                            case "Bulten_No": exchangeModel.Bulten = attribute.Value.ToString(); break;
                            default:
                                break;
                        }
                    }

                    foreach (XmlNode currencyNode in tarihDateNode.ChildNodes)
                    {
                        Currency currencyModel = new Currency();

                        if (currencyNode.Name == "Currency")
                        {
                            foreach (XmlNode subNode in currencyNode.ChildNodes)
                            {
                                switch (subNode.Name)
                                {
                                    case "Unit": currencyModel.Unit = Convert.ToInt32(subNode.InnerText); break;
                                    case "Isim": currencyModel.Isim = subNode.InnerText; break;
                                    case "CurrencyName": currencyModel.CurrencyName = subNode.InnerText; break;
                                    case "ForexBuying": currencyModel.ForexBuying = (!string.IsNullOrEmpty(subNode.InnerText) ? Convert.ToDecimal(subNode.InnerText) : default(decimal?)); break;
                                    case "ForexSelling": currencyModel.ForexSelling = (!string.IsNullOrEmpty(subNode.InnerText) ? Convert.ToDecimal(subNode.InnerText) : default(decimal?)); break;
                                    case "BanknoteBuying": currencyModel.BanknoteBuying = (!string.IsNullOrEmpty(subNode.InnerText) ? Convert.ToDecimal(subNode.InnerText) : default(decimal?)); break;
                                    case "BanknoteSelling": currencyModel.BanknoteSelling = (!string.IsNullOrEmpty(subNode.InnerText) ? Convert.ToDecimal(subNode.InnerText) : default(decimal?)); break;
                                    case "CrossRateUSD": currencyModel.CrossRateUSD = (!string.IsNullOrEmpty(subNode.InnerText) ? Convert.ToDecimal(subNode.InnerText) : default(decimal?)); break;
                                    case "CrossRateOther": currencyModel.CrossRateOther = (!string.IsNullOrEmpty(subNode.InnerText) ? Convert.ToDecimal(subNode.InnerText) : default(decimal?)); break;
                                }
                            }
                        }

                        exchangeModel.Tarih_Date.Currencies.Add(currencyModel);
                    }
                }
            }

            return foundTarihDateNode ? exchangeModel : null;
        }
    }
}
