using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Caching.Redis.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Mock.Factories;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Routing.Providers.Mock;
using Infrastructure.Security.Authentication.Mock;
using Infrastructure.Transaction.UnitOfWork.Sql.Mock;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.AA.Configuration.Mapping;
using Services.Api.Business.Departments.AA.Services;
using Services.Communication.Http.Broker.Localization.Mock;
using Services.Communication.Mq.Rabbit.Publisher.Mock;

using Test.Services.Api.Business.Departments.AA.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.AA.Factories.Repositories;

namespace Test.Services.Api.Business.Departments.AA.Factories.Services
{
    public class InventoryServiceFactory
    {
        private static InventoryService service;

        public static InventoryService Instance
        {
            get
            {
                if (service == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();

                    service = new InventoryService(
                        mapper: MappingFactory.GetInstance(new MappingProfile()),
                        unitOfWork: UnitOfWorkFactory.GetInstance(configuration),
                        redisCacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                        createInventoryRequestPublisher: CreateInventoryRequestPublisherProvider.GetCreateInventoryRequestPublisher(
                            configuration: CreateInventoryRequestRabbitConfigurationProvider.GetCreateInventoryRequestPublisher(configuration)),
                        transactionItemRepository: TransactionItemRepositoryFactory.Instance,
                        transactionRepository: TransactionRepositoryFactory.Instance,
                        inventoryRepository: InventoryRepositoryFactory.Instance,
                        inventoryDefaultsRepository: InventoryDefaultsRepositoryFactory.Instance,
                        pendingWorkerInventoryRepository: PendingWorkerInventoryRepositoryFactory.Instance,
                        workerInventoryRepository: WorkerInventoryRepositoryFactory.Instance,
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
