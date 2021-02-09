using System.Configuration;

namespace MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator.Providers
{
    /// <summary>
    /// Servis rotalarına ait endpoint isimlerini sağlayan sınıf
    /// </summary>
    public class RouteNameProvider
    {
        /// <summary>
        /// Kullanıcı bilgilerine göre token getirir
        /// </summary>
        public string Auth_GetToken
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Configuration.Authorization.Endpoints.GetToken"]
                    .ToString();
            }
        }

        /// <summary>
        /// Token bilgisine göre kullanıcı bilgisini getirir
        /// </summary>
        public string Auth_GetUser
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Configuration.Authorization.Endpoints.GetUser"]
                    .ToString();
            }
        }

        public string SampleDataProvider_GetData
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.SampleDataProvider.GetData"]
                    .ToString();
            }
        }

        public string SampleDataProvider_PostData
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.SampleDataProvider.PostData"]
                    .ToString();
            }
        }

        /// <summary>
        /// Çalışan için banka hesabı oluşturur
        /// </summary>
        public string Accounting_CreateBankAccount
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.Accounting.CreateBankAccount"]
                    .ToString();
            }
        }

        public string Accounting_GetBankAccountsOfWorker
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.Accounting.GetBankAccountsOfWorker"]
                    .ToString();
            }
        }

        public string Accounting_GetCurrencies
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.Accounting.GetCurrencies"]
                    .ToString();
            }
        }

        public string Accounting_CreateCurrency
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.Accounting.CreateCurrency"]
                    .ToString();
            }
        }
    }
}
