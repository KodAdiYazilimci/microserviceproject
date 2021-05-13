using Infrastructure.Mock.Factories;
using Infrastructure.Mock.Publishers;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.HR.Configuration.Mapping;
using Services.Business.Departments.HR.Services;

using Test.Services.Business.Departments.HR.Factories.Infrastructure;
using Test.Services.Business.Departments.HR.Factories.Repositories;

namespace Test.Services.Business.Departments.HR.Factories.Services
{
    public class PersonServiceFactory
    {
        private static PersonService service = null;

        public static PersonService Instance
        {
            get
            {
                if (service == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();

                    service = new PersonService(
                        mapper: MappingFactory.GetInstance(new MappingProfile()),
                        routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                        serviceCommunicator: ServiceCommunicatorFactory.GetServiceCommunicator(
                            memoryCache: MemoryCacheFactory.Instance,
                            credentialProvider: CredentialProviderFactory.GetCredentialProvider(configuration),
                            routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                            serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(configuration)),
                        AAassignInventoryToWorkerPublisher: AAAssignInventoryToWorkerPublisherProvider.GetPublisher(
                            configuration: AAAssignInventoryToWorkerRabbitConfigurationProvider.GetConfiguration(configuration)),
                        ITassignInventoryToWorkerPublisher: ITassignInventoryToWorkerPublisherProvider.GetPublisher(
                            configuration: ITAssignInventoryToWorkerRabbitConfigurationProvider.GetConfiguration(configuration)),
                        createBankAccountPublisher: CreateBankAccountPublisherProvider.GetPublisher(
                            rabbitConfiguration: CreateBankAccountRabbitConfigurationProvider.GetConfiguration(configuration)),
                        unitOfWork: UnitOfWorkFactory.GetInstance(configuration),
                        translationProvider: TranslationProviderFactory.GetTranslationProvider(
                            configuration: configuration,
                            cacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                            translationRepository: TranslationRepositoryFactory.GetTranslationRepository(
                                translationDbContext: TranslationDbContextFactory.GetTranslationDbContext(configuration)),
                            translationHelper: TranslationHelperFactory.Instance),
                        cacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                        transactionRepository: TransactionRepositoryFactory.Instance,
                        transactionItemRepository: TransactionItemRepositoryFactory.Instance,
                        departmentRepository: DepartmentRepositoryFactory.Instance,
                        personRepository: PersonRepositoryFactory.Instance,
                        titleRepository: TitleRepositoryFactory.Instance,
                        workerRepository: WorkerRepositoryFactory.Instance,
                        workerRelationRepository: WorkerRelationRepositoryFactory.Instance);
                }

                return service;
            }
        }
    }
}
