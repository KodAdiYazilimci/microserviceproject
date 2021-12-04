using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Caching.Redis.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Mock.Factories;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Routing.Providers.Mock;
using Infrastructure.Security.Authentication.Mock;
using Infrastructure.Transaction.UnitOfWork.Sql.Mock;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.Accounting.Configuration.Mapping;
using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Localization.Mock;

using Test.Services.Api.Business.Departments.Accounting.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.Accounting.Factories.Repositories;

namespace Test.Services.Api.Business.Departments.Accounting.Factories.Services
{
    public class BankServiceFactory
    {
        private static BankService service;

        public static BankService Instance
        {
            get
            {
                if (service == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();

                    service = new BankService(
                        mapper: MappingFactory.GetInstance(new MappingProfile()),
                        unitOfWork: UnitOfWorkFactory.GetInstance(configuration),
                        redisCacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                        transactionItemRepository: TransactionItemRepositoryFactory.Instance,
                        transactionRepository: TransactionRepositoryFactory.Instance,
                        bankAccountRepository: BankAccountRepositoryFactory.Instance,
                        currencyRepository: CurrencyRepositoryFactory.Instance,
                        salaryPaymentRepository: SalaryPaymentRepositoryFactory.Instance,
                        localizationCommunicator: LocalizationCommunicatorProvider.GetLocalizationCommunicator(
                            routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                            serviceCommunicator: ServiceCommunicatorFactory.GetServiceCommunicator(
                                cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                                credentialProvider: CredentialProviderFactory.GetCredentialProvider(configuration),
                                routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                                serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(configuration))));
                }

                return service;
            }
        }
    }
}
