using Infrastructure.Mock.Factories;
using Infrastructure.Mock.Providers.Communicators;
using Infrastructure.Mock.Publishers;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.HR.Configuration.Mapping;
using Services.Api.Business.Departments.HR.Services;

using Test.Services.Api.Business.Departments.HR.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.HR.Factories.Repositories;

namespace Test.Services.Api.Business.Departments.HR.Factories.Services
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
                        aACommunicator: AACommunicatorProvider.GetAACommunicator(
                             routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                             serviceCommunicator: ServiceCommunicatorFactory.GetServiceCommunicator(
                                 cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                                 credentialProvider: CredentialProviderFactory.GetCredentialProvider(configuration),
                                 routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                                 serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(configuration))),
                        accountingCommunicator: AccountingCommunicatorProvider.GetAccountingCommunicator(
                             routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                             serviceCommunicator: ServiceCommunicatorFactory.GetServiceCommunicator(
                                 cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                                 credentialProvider: CredentialProviderFactory.GetCredentialProvider(configuration),
                                 routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                                 serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(configuration))),
                        itCommunicator: ITCommunicatorProvider.GetITCommunicator(
                             routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                             serviceCommunicator: ServiceCommunicatorFactory.GetServiceCommunicator(
                                 cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                                 credentialProvider: CredentialProviderFactory.GetCredentialProvider(configuration),
                                 routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                                 serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(configuration))),
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
                        redisCacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
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
