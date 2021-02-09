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

        /// <summary>
        /// Departmanları verir
        /// </summary>
        public string HR_GetDepartments
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.HR.GetDepartments"]
                    .ToString();
            }
        }

        /// <summary>
        /// Yeni departman oluşturur
        /// </summary>
        public string HR_CreateDepartment
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.HR.CreateDepartment"]
                    .ToString();
            }
        }

        /// <summary>
        /// Kişi listesini verir
        /// </summary>
        public string HR_GetPeople
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.HR.GetPeople"]
                    .ToString();
            }
        }

        /// <summary>
        /// Kişi oluşturur
        /// </summary>
        public string HR_CreatePerson
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.HR.CreatePerson"]
                    .ToString();
            }
        }

        /// <summary>
        /// Ünvanları verir
        /// </summary>
        public string HR_GetTitles
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.HR.GetTitles"]
                    .ToString();
            }
        }

        /// <summary>
        /// Yeni bir ünvan oluşturur
        /// </summary>
        public string HR_CreateTitle
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.HR.CreateTitle"]
                    .ToString();
            }
        }

        /// <summary>
        /// Çalışanları verir
        /// </summary>
        public string HR_GetWorkers
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.HR.GetWorkers"]
                    .ToString();
            }
        }

        /// <summary>
        /// Yeni bir çalışan oluşturur
        /// </summary>
        public string HR_CreateWorker
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.HR.CreateWorker"]
                    .ToString();
            }
        }
    }
}
