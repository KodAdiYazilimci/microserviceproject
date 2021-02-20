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

        /// <summary>
        /// IT envanterlerini verir
        /// </summary>
        public string IT_GetInventories
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.IT.GetInventories"]
                    .ToString();
            }
        }

        /// <summary>
        /// İşe yeni başlayan çalışanlar için IT envanterlerini verir
        /// </summary>
        public string IT_GetInventoriesForNewWorker
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.IT.GetInventoriesForNewWorker"]
                    .ToString();
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
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.IT.CreateInventory"]
                    .ToString();
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
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.IT.AssignInventoryToWorker"]
                    .ToString();
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
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.IT.CreateDefaultInventoryForNewWorker"]
                    .ToString();
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
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.AA.GetInventories"]
                    .ToString();
            }
        }

        /// <summary>
        /// İşe yeni başlayan çalışanlar için İdari işler envanterlerini verir
        /// </summary>
        public string AA_GetInventoriesForNewWorker
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.AA.GetInventoriesForNewWorker"]
                    .ToString();
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
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.AA.CreateInventory"]
                    .ToString();
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
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.AA.AssignInventoryToWorker"]
                    .ToString();
            }
        }

        /// <summary>
        /// Yeni çalışan için varsayılan idari işler envanteri ataması yapar
        /// </summary>
        public string AA_CreateDefaultInventoryForNewWorker
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Services.Endpoints.AA.CreateDefaultInventoryForNewWorker"]
                    .ToString();
            }
        }
    }
}
