using AutoMapper;

using Infrastructure.Caching.Abstraction;
using Infrastructure.Caching.Redis.Mock;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Repositories;
using Infrastructure.Localization.Translation.Persistence.Mock.EntityFramework.Persistence;
using Infrastructure.Localization.Translation.Provider.Mock;
using Infrastructure.Mock.Factories;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.AA.Configuration.Mapping;
using Services.Api.Business.Departments.AA.Configuration.Persistence;
using Services.Api.Business.Departments.AA.Services;
using Services.Communication.Mq.Queue.Buying.Configuration.Mock;
using Services.Communication.Mq.Queue.Buying.Rabbit.Publishers.Mock;

using Test.Services.Api.Business.Departments.AA.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.AA.Factories.Repositories;

namespace Test.Services.Api.Business.Departments.AA.Factories.Services
{
    public class InventoryServiceFactory
    {
        public static InventoryService Instance
        {
            get
            {
                IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                IMapper mapper = MappingFactory.GetInstance(new MappingProfile());
                IDistrubutedCacheProvider redisCacheDataProvider = CacheDataProviderFactory.GetInstance(configuration);
                ISqlUnitOfWork unitOfWork = new UnitOfWork(configuration);

                var service = new InventoryService(
                    mapper: mapper,
                    unitOfWork: unitOfWork,
                    redisCacheDataProvider: redisCacheDataProvider,
                    translationProvider: TranslationProviderFactory.GetTranslationProvider(
                        configuration: configuration,
                        cacheDataProvider: redisCacheDataProvider,
                        translationRepository: new EfTranslationRepository(TranslationDbContextFactory.GetTranslationDbContext(configuration)),
                        translationHelper: TranslationHelperFactory.Instance),
                    createInventoryRequestPublisher: CreateInventoryRequestPublisherProvider.GetCreateInventoryRequestPublisher(
                        configuration: CreateInventoryRequestRabbitConfigurationProvider.GetCreateInventoryRequestPublisher(configuration)),
                    transactionItemRepository: TransactionItemRepositoryFactory.GetInstance(unitOfWork),
                    transactionRepository: TransactionRepositoryFactory.GetInstance(unitOfWork),
                    inventoryRepository: InventoryRepositoryFactory.GetInstance(unitOfWork),
                    inventoryDefaultsRepository: InventoryDefaultsRepositoryFactory.GetInstance(unitOfWork),
                    pendingWorkerInventoryRepository: PendingWorkerInventoryRepositoryFactory.GetInstance(unitOfWork),
                    workerInventoryRepository: WorkerInventoryRepositoryFactory.GetInstance(unitOfWork));

                service.TransactionIdentity = new Random().Next(int.MinValue, int.MaxValue).ToString();

                return service;
            }
        }
    }
}
