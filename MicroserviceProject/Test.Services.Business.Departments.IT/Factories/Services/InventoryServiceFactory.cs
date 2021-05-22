using Infrastructure.Localization.Repositories;
using Infrastructure.Mock.Factories;
using Infrastructure.Mock.Providers.Publishers;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.IT.Configuration.Mapping;
using Services.Business.Departments.IT.Services;

using Test.Services.Business.Departments.IT.Factories.Infrastructure;
using Test.Services.Business.Departments.IT.Factories.Repositories;

namespace Test.Services.Business.Departments.IT.Factories.Services
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
                        translationProvider: TranslationProviderFactory.GetTranslationProvider(
                            configuration: configuration,
                            cacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                            translationRepository: new TranslationRepository(TranslationDbContextFactory.GetTranslationDbContext(configuration)),
                            translationHelper: TranslationHelperFactory.Instance),
                        createInventoryRequestPublisher: CreateInventoryRequestPublisherProvider.GetCreateInventoryRequestPublisher(
                            configuration: CreateInventoryRequestRabbitConfigurationProvider.GetCreateInventoryRequestPublisher(configuration)),
                        transactionItemRepository: TransactionItemRepositoryFactory.Instance,
                        transactionRepository: TransactionRepositoryFactory.Instance,
                        inventoryRepository: InventoryRepositoryFactory.Instance,
                        inventoryDefaultsRepository: InventoryDefaultsRepositoryFactory.Instance,
                        pendingWorkerInventoryRepository: PendingWorkerInventoryRepositoryFactory.Instance,
                        workerInventoryRepository: WorkerInventoryRepositoryFactory.Instance);
                }

                return service;
            }
        }
    }
}
