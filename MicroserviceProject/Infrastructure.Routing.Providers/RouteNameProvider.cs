using Microsoft.Extensions.Configuration;

using System;

namespace Infrastructure.Routing.Providers
{
    /// <summary>
    /// Servis rotalarına ait endpoint isimlerini sağlayan sınıf
    /// </summary>
    public class RouteNameProvider : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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
                    .GetSection("Endpoints")["GetToken"];
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
                    .GetSection("Endpoints")["GetUser"];
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
                    .GetSection("HR")["GetDepartments"];
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
                    .GetSection("HR")["CreateDepartment"];
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
                    .GetSection("HR")["GetPeople"];
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
                    .GetSection("HR")["CreatePerson"];
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
                    .GetSection("HR")["GetTitles"];
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
                    .GetSection("HR")["CreateTitle"];
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
                    .GetSection("HR")["GetWorkers"];
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
                    .GetSection("HR")["CreateWorker"];
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
                    .GetSection("Accounting")["CreateBankAccount"];
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
                    .GetSection("Accounting")["GetBankAccountsOfWorker"];
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
                    .GetSection("Accounting")["GetCurrencies"];
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
                    .GetSection("Accounting")["CreateCurrency"];
            }
        }

        /// <summary>
        /// Çalışanın maaş ödemelerini verir
        /// </summary>
        public string Accounting_GetSalaryPaymentsOfWorker
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Accounting")["GetSalaryPaymentsOfWorker"];
            }
        }


        /// <summary>
        /// Çalışana maaş ödemesi oluşturur
        /// </summary>
        public string Accounting_CreateSalaryPayment
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Accounting")["CreateSalaryPayment"];
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
                    .GetSection("IT")["GetInventories"];
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
                    .GetSection("IT")["CreateInventory"];
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
                    .GetSection("IT")["AssignInventoryToWorker"];
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
                    .GetSection("IT")["GetInventoriesForNewWorker"];
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
                    .GetSection("IT")["CreateDefaultInventoryForNewWorker"];
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
                    .GetSection("AA")["GetInventories"];
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
                    .GetSection("AA")["CreateInventory"];
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
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("AA")["CreateDefaultInventoryForNewWorker"];
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
                    .GetSection("AA")["AssignInventoryToWorker"];
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
                    .GetSection("AA")["GetInventoriesForNewWorker"];
            }
        }

        /// <summary>
        /// İdari işlerin bekleyen satın alımları için bilgilendirme yapar
        /// </summary>
        public string AA_InformInventoryRequest
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("AA")["InformInventoryRequest"];
            }
        }

        /// <summary>
        /// Bilgi teknolojileri departmanının bekleyen satın alımları için bilgilendirme yapar
        /// </summary>
        public string IT_InformInventoryRequest
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("IT")["InformInventoryRequest"];
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
                    .GetSection("AA")["RollbackTransaction"];
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
                    .GetSection("Accounting")["RollbackTransaction"];
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
                    .GetSection("HR")["RollbackTransaction"];
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
                    .GetSection("IT")["RollbackTransaction"];
            }
        }

        /// <summary>
        /// Satınalma departmanındaki envanter taleplerini getirir
        /// </summary>
        public string Buying_GetInventoryRequests
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Buying")["GetInventoryRequests"];
            }
        }

        /// <summary>
        /// Satınalma departmanına envanter talebi oluşturur
        /// </summary>
        public string Buying_CreateInventoryRequest
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Buying")["CreateInventoryRequest"];
            }
        }

        /// <summary>
        /// İşlemi geri alır
        /// </summary>
        public string Buying_RollbackTransaction
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Buying")["RollbackTransaction"];
            }
        }

        /// <summary>
        /// Satın alınması planlanan envantere ait masrafın sonuçlandırılmasını sağlar
        /// </summary>
        public string Buying_ValidateCostInventory
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Buying")["ValidateCostInventory"];
            }
        }

        /// <summary>
        /// Finans departmanındaki karar verilen masrafları verir
        /// </summary>
        public string Finance_GetDecidedCosts
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Finance")["GetDecidedCosts"];
            }
        }

        /// <summary>
        /// Finans departmanı için masraf kararı oluşturur
        /// </summary>
        public string Finance_CreateCost
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Finance")["CreateCost"];
            }
        }

        /// <summary>
        /// Finans departmanı için ürün üretim kararı oluşturur
        /// </summary>
        public string Finance_CreateProductionRequest
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Finance")["CreateProductionRequest"];
            }
        }

        /// <summary>
        /// Finans departmanındaki masrafa onay veya red verir
        /// </summary>
        public string Finance_DecideCost
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Finance")["DecideCost"];
            }
        }

        public string CR_GetCustomers
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("CR")["GetCustomers"];
            }
        }
        public string CR_CreateCustomer
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("CR")["CreateCustomer"];
            }
        }

        public string Production_GetProducts
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Production")["GetProducts"];
            }
        }

        public string Production_CreateProduct
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Production")["CreateProduct"];
            }
        }

        public string Production_ProduceProduct
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Production")["ProduceProduct"];
            }
        }

        public string Production_ReEvaluateProduceProduct
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Production")["ReEvaluateProduceProduct"];
            }
        }

        public string Selling_GetSolds
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Selling")["GetSolds"];
            }
        }

        public string Selling_CreateSelling
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Selling")["CreateSelling"];
            }
        }

        public string Selling_NotifyProductionRequest
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Selling")["NotifyProductionRequest"];
            }
        }        

        public string Storage_GetStock
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Storage")["GetStock"];
            }
        }

        public string Storage_DescendProductStock
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Storage")["DescendProductStock"];
            }
        }

        public string Storage_CreateStock
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("Storage")["CreateStock"];
            }
        }

        /// <summary>
        /// Güvenlik web soketine bildirim mesajı gönderir
        /// </summary>
        public string WebSocketSecurity_SendTokenNotification
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("WebSocket.Security")["SendTokenNotification"];
            }
        }

        /// <summary>
        /// Sistem işleyişi web soketine bildirim mesajı gönderir
        /// </summary>
        public string WebSocketSecurity_SendErrorNofication
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("WebSocket.Reliability")["SendErrorNofication"];
            }
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {

                }

                disposed = true;
            }
        }
    }
}
