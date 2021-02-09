using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Infrastructure.Communication.Moderator.Providers
{
    /// <summary>
    /// Servis rotalarına ait endpoint isimlerini sağlayan sınıf
    /// </summary>
    public class RouteNameProvider
    {
        /// <summary>
        /// Endpoint isimlerini getiren configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Servis rotalarına ait endpoint isimlerini sağlayan sınıf
        /// </summary>
        /// <param name="configuration">Endpoint isimlerini getiren configuration</param>
        public RouteNameProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Kullanıcı bilgilerine göre token getirir
        /// </summary>
        public string Auth_GetToken
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("Endpoints")
                    .GetSection("GetToken").Value;
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
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("Endpoints")
                    .GetSection("GetUser").Value;
            }
        }

        public string SampleDataProvider_GetData
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("SampleDataProvider")
                    .GetSection("GetData").Value;
            }
        }

        public string SampleDataProvider_PostData
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("SampleDataProvider")
                    .GetSection("PostData").Value;
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
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("HR")
                    .GetSection("GetDepartments").Value;
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
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("HR")
                    .GetSection("CreateDepartment<").Value;
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
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("HR")
                    .GetSection("GetPeople").Value;
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
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("HR")
                    .GetSection("CreatePerson").Value;
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
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("HR")
                    .GetSection("GetTitles").Value;
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
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("HR")
                    .GetSection("CreateTitle").Value;
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
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("HR")
                    .GetSection("GetWorkers").Value;
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
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("HR")
                    .GetSection("CreateWorker").Value;
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
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Accounting")
                    .GetSection("CreateBankAccount").Value;
            }
        }

        public string Accounting_GetBankAccountsOfWorker
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Accounting")
                    .GetSection("GetBankAccountsOfWorker").Value;
            }
        }

        public string Accounting_GetCurrencies
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Accounting")
                    .GetSection("GetCurrencies").Value;
            }
        }

        public string Accounting_CreateCurrency
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Accounting")
                    .GetSection("CreateCurrency").Value;
            }
        }
    }
}
