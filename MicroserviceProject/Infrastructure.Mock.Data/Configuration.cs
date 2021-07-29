using Infrastructure.Mock.Providers.Configuration;
using Infrastructure.Mock.Providers.Configuration.Sections.AuthorizationNode;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Data
{
    /// <summary>
    /// Yapılandırma ayarları sınıfı
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Yapılandırma ayarlarını taklit eden sınıf
        /// </summary>
        private static AppConfigurationProvider appConfigurationProvider = null;

        /// <summary>
        /// Yapılandırma ayarlarına dair veriler verir
        /// </summary>
        /// <param name="authorizationCredential">Credential düğüm sınıfı nesnesi</param>
        /// <param name="loggingFilePath">Dosya log yolu</param>
        /// <returns></returns>
        public static IConfiguration GetConfiguration(CredentialSection authorizationCredential, string loggingFilePath)
        {
            if (appConfigurationProvider == null)
            {
                appConfigurationProvider = new AppConfigurationProvider();

                appConfigurationProvider.ConfigurationSection.AuthorizationSection.DataSourceSection["ConnectionString"] = "server=localhost;DataBase=Microservice_Security_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true";
                appConfigurationProvider.ConfigurationSection.AuthorizationSection.CredentialSection = authorizationCredential;
                appConfigurationProvider.ConfigurationSection.AuthorizationSection.EndpointsSection["GetToken"] = "authorization.auth.gettoken";
                appConfigurationProvider.ConfigurationSection.AuthorizationSection.EndpointsSection["GetUser"] = "authorization.auth.getuser";
                appConfigurationProvider.ConfigurationSection.AuthorizationSection.JwtSection["JWTSecretKey"] = "Srkn$99@2021!+999**&%100-S3cReTKeY#$$";
                appConfigurationProvider.ConfigurationSection.AuthorizationSection.JwtSection["Expiration"] = "60";
                appConfigurationProvider.ConfigurationSection.AuthorizationSection.JwtSection["Issuer"] = "Issuer";
                appConfigurationProvider.ConfigurationSection.AuthorizationSection.JwtSection["Audience"] = "Audience";

                appConfigurationProvider.ConfigurationSection.LocalizationSection["TranslationDbConnnectionString"] = "server=localhost;DataBase=Microservice_Localization_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true";
                appConfigurationProvider.ConfigurationSection.LocalizationSection["DefaultRegion"] = "tr-TR";
                appConfigurationProvider.ConfigurationSection.LocalizationSection["CacheKey"] = "localization.translations";

                appConfigurationProvider.ConfigurationSection.LoggingSection.RequestResponseLoggingSection.DataBaseConfigurationSection["DataSource"] = "server=localhost;DataBase=Microservice_Logs_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true";
                appConfigurationProvider.ConfigurationSection.LoggingSection.RequestResponseLoggingSection.FileConfigurationSection["Path"] = loggingFilePath;
                appConfigurationProvider.ConfigurationSection.LoggingSection.RequestResponseLoggingSection.FileConfigurationSection["FileName"] = "RequestResponseLog";
                appConfigurationProvider.ConfigurationSection.LoggingSection.RequestResponseLoggingSection.FileConfigurationSection["Encoding"] = "UTF-8";
                appConfigurationProvider.ConfigurationSection.LoggingSection.RequestResponseLoggingSection.MongoConfigurationSection["ConnectionString"] = "mongodb://localhost:27017";
                appConfigurationProvider.ConfigurationSection.LoggingSection.RequestResponseLoggingSection.MongoConfigurationSection["DataBase"] = "Logs";
                appConfigurationProvider.ConfigurationSection.LoggingSection.RequestResponseLoggingSection.MongoConfigurationSection["CollectionName"] = "RequestResponseLog";

                appConfigurationProvider.ConfigurationSection.RoutingSection["DataSource"] = "server=localhost;DataBase=Microservice_Routing_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true";

                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.HostSection["DefaultHost"] = "localhost";
                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.HostSection["DefaultUserName"] = "guest";
                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.HostSection["DefaultPassword"] = "guest";
                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.ServicesSection.AASection.QueueNamesSection["AssignInventoryToWorker"] = "aa.queue.inventory.assignInventorytoworker";
                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.ServicesSection.AASection.QueueNamesSection["InformInventoryRequest"] = "aa.queue.request.informinventoryrequest";
                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.ServicesSection.ITSection.QueueNamesSection["AssignInventoryToWorker"] = "it.queue.inventory.assignInventorytoworker";
                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.ServicesSection.ITSection.QueueNamesSection["InformInventoryRequest"] = "it.queue.request.informinventoryrequest";
                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.ServicesSection.AccountingSection.QueueNamesSection["CreateBankAccount"] = "accounting.queue.bankaccounts.createbankaccount";
                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.ServicesSection.AccountingSection.QueueNamesSection["CreateInventoryRequest"] = "buying.queue.request.createinventoryrequest";
                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.ServicesSection.AccountingSection.QueueNamesSection["NotifyCostApprovement"] = "buying.queue.cost.notifycostapprovement";
                appConfigurationProvider.ConfigurationSection.RabbitQueuesSection.ServicesSection.FinanceSection.QueueNamesSection["InventoryRequest"] = "finance.queue.request.inventoryrequest";

                appConfigurationProvider.ConfigurationSection.WebSocketsSection["DataSource"] = "server=localhost;DataBase=Microservice_Socket_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true";

                appConfigurationProvider.ServicesSection.EndpointsSection.AASection["GetInventories"] = "aa.inventory.getinventories";
                appConfigurationProvider.ServicesSection.EndpointsSection.AASection["CreateInventory"] = "aa.inventory.createinventory";
                appConfigurationProvider.ServicesSection.EndpointsSection.AASection["CreateDefaultInventoryForNewWorker"] = "aa.inventory.createdefaultinventoryfornewworker";
                appConfigurationProvider.ServicesSection.EndpointsSection.AASection["AssignInventoryToWorker"] = "aa.inventory.assigninventorytoworker";
                appConfigurationProvider.ServicesSection.EndpointsSection.AASection["GetInventoriesForNewWorker"] = "aa.inventory.getinventoriesfornewworker";
                appConfigurationProvider.ServicesSection.EndpointsSection.AASection["CreateDefaultInventoryForNewWorker"] = "aa.inventory.createdefaultinventoryfornewworker";
                appConfigurationProvider.ServicesSection.EndpointsSection.AASection["RollbackTransaction"] = "aa.transaction.rollbacktransaction";
                appConfigurationProvider.ServicesSection.EndpointsSection.AASection["InformInventoryRequest"] = "aa.inventory.informinventoryrequest";

                appConfigurationProvider.ServicesSection.EndpointsSection.AccountingSection["CreateBankAccount"] = "accounting.bankaccounts.createbankaccount";
                appConfigurationProvider.ServicesSection.EndpointsSection.AccountingSection["GetBankAccountsOfWorker"] = "accounting.bankaccounts.getbankaccountsofworker";
                appConfigurationProvider.ServicesSection.EndpointsSection.AccountingSection["GetCurrencies"] = "accounting.bankaccounts.getcurrencies";
                appConfigurationProvider.ServicesSection.EndpointsSection.AccountingSection["CreateCurrency"] = "accounting.bankaccounts.createcurrency";
                appConfigurationProvider.ServicesSection.EndpointsSection.AccountingSection["RollbackTransaction"] = "accounting.transaction.rollbacktransaction";

                appConfigurationProvider.ServicesSection.EndpointsSection.BuyingSection["GetInventoryRequests"] = "buying.request.getinventoryrequests";
                appConfigurationProvider.ServicesSection.EndpointsSection.BuyingSection["CreateInventoryRequest"] = "buying.request.createinventoryrequest";
                appConfigurationProvider.ServicesSection.EndpointsSection.BuyingSection["RollbackTransaction"] = "buying.transaction.rollbacktransaction";
                appConfigurationProvider.ServicesSection.EndpointsSection.BuyingSection["ValidateCostInventory"] = "buying.request.validatecostinventory";

                appConfigurationProvider.ServicesSection.EndpointsSection.FinanceSection["GetDecidedCosts"] = "finance.cost.getdecidedcosts";
                appConfigurationProvider.ServicesSection.EndpointsSection.FinanceSection["CreateCost"] = "finance.cost.createcost";
                appConfigurationProvider.ServicesSection.EndpointsSection.FinanceSection["DecideCost"] = "finance.cost.decidecost";

                appConfigurationProvider.ServicesSection.EndpointsSection.HRSection["GetDepartments"] = "hr.department.getdepartments";
                appConfigurationProvider.ServicesSection.EndpointsSection.HRSection["CreateDepartment"] = "hr.department.createdepartment";
                appConfigurationProvider.ServicesSection.EndpointsSection.HRSection["GetPeople"] = "hr.person.getpeople";
                appConfigurationProvider.ServicesSection.EndpointsSection.HRSection["CreatePerson"] = "hr.person.createperson";
                appConfigurationProvider.ServicesSection.EndpointsSection.HRSection["GetTitles"] = "hr.person.gettitles";
                appConfigurationProvider.ServicesSection.EndpointsSection.HRSection["CreateTitle"] = "hr.person.createtitle";
                appConfigurationProvider.ServicesSection.EndpointsSection.HRSection["GetWorkers"] = "hr.person.getworkers";
                appConfigurationProvider.ServicesSection.EndpointsSection.HRSection["CreateWorker"] = "hr.person.createworker";
                appConfigurationProvider.ServicesSection.EndpointsSection.HRSection["RollbackTransaction"] = "hr.transaction.rollbacktransaction";

                appConfigurationProvider.ServicesSection.EndpointsSection.ITSection["GetInventories"] = "it.inventory.getinventories";
                appConfigurationProvider.ServicesSection.EndpointsSection.ITSection["CreateInventory"] = "it.inventory.createinventory";
                appConfigurationProvider.ServicesSection.EndpointsSection.ITSection["AssignInventoryToWorker"] = "it.inventory.assigninventorytoworker";
                appConfigurationProvider.ServicesSection.EndpointsSection.ITSection["GetInventoriesForNewWorker"] = "it.inventory.getinventoriesfornewworker";
                appConfigurationProvider.ServicesSection.EndpointsSection.ITSection["CreateDefaultInventoryForNewWorker"] = "it.inventory.createdefaultinventoryfornewworker";
                appConfigurationProvider.ServicesSection.EndpointsSection.ITSection["RollbackTransaction"] = "it.transaction.rollbacktransaction";
                appConfigurationProvider.ServicesSection.EndpointsSection.ITSection["InformInventoryRequest"] = "it.inventory.informinventoryrequest";

                appConfigurationProvider.ServicesSection.EndpointsSection.WebSocketsSecuritySection["SendTokenNotification"] = "websockets.security.sendtokennotification";
                appConfigurationProvider.ServicesSection.EndpointsSection.WebSocketsSecuritySection["SendErrorNotification"] = "websockets.reliability.senderrornotification";

                appConfigurationProvider.PersistenceSection["DataSource"] = "server=localhost;DataBase=Microservice_HR_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true";
                appConfigurationProvider.PersistenceSection["CacheServer"] = "localhost";

                appConfigurationProvider.WebSocketsSection.EndpointsSection["TokensHub.GetTokenMessages"] = "websockets.security.tokenshub.gettokenmessages";
            }

            return appConfigurationProvider;
        }
    }
}
