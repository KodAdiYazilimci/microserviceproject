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

        /// <summary>
        /// Çalışanın banka hesaplarını getirir
        /// </summary>
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

        /// <summary>
        /// Para birimlerini getirir
        /// </summary>
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

        /// <summary>
        /// Para birimi oluşturur
        /// </summary>
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

        /// <summary>
        /// IT envanterlerini verir
        /// </summary>
        public string IT_GetInventories
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("IT")
                    .GetSection("GetInventories").Value;
            }
        }

        /// <summary>
        /// IT envanteri oluşturur
        /// </summary>
        public string IT_CreateInventory
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("IT")
                    .GetSection("CreateInventory").Value;
            }
        }

        /// <summary>
        /// Çalışana IT envanteri ataması yapar
        /// </summary>
        public string IT_AssignInventoryToWorker
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("IT")
                    .GetSection("AssignInventoryToWorker").Value;
            }
        }

        /// <summary>
        /// Yeni çalışanlar için IT tarafından varsayılan envanterleri verir
        /// </summary>
        public string IT_GetInventoriesForNewWorker
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("IT")
                    .GetSection("GetInventoriesForNewWorker").Value;
            }
        }


        /// <summary>
        /// Yeni çalışan için varsayılan IT envanteri ataması yapar
        /// </summary>
        public string IT_CreateDefaultInventoryForNewWorker
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("IT")
                    .GetSection("CreateDefaultInventoryForNewWorker").Value;
            }
        }

        /// <summary>
        /// İdari işler envanterlerini verir
        /// </summary>
        public string AA_GetInventories
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("AA")
                    .GetSection("GetInventories").Value;
            }
        }

        /// <summary>
        /// İdari işler envanteri oluşturur
        /// </summary>
        public string AA_CreateInventory
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("AA")
                    .GetSection("CreateInventory").Value;
            }
        }

        /// <summary>
        /// Çalışana idari işler envanteri ataması yapar
        /// </summary>
        public string AA_AssignInventoryToWorker
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("AA")
                    .GetSection("AssignInventoryToWorker").Value;
            }
        }

        /// <summary>
        /// Yeni çalışanlar için idari işler tarafından varsayılan envanterleri verir
        /// </summary>
        public string AA_GetInventoriesForNewWorker
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("AA")
                    .GetSection("GetInventoriesForNewWorker").Value;
            }
        }

        /// <summary>
        /// İşlemi geri alır
        /// </summary>
        public string AA_RollbackTransaction
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("AA")
                    .GetSection("RollbackTransaction").Value;
            }
        }

        /// <summary>
        /// İşlemi geri alır
        /// </summary>
        public string Accounting_RollbackTransaction
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Accounting")
                    .GetSection("RollbackTransaction").Value;
            }
        }

        /// <summary>
        /// İşlemi geri alır
        /// </summary>
        public string HR_RollbackTransaction
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("HR")
                    .GetSection("RollbackTransaction").Value;
            }
        }

        /// <summary>
        /// İşlemi geri alır
        /// </summary>
        public string IT_RollbackTransaction
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("IT")
                    .GetSection("RollbackTransaction").Value;
            }
        }
    }
}
