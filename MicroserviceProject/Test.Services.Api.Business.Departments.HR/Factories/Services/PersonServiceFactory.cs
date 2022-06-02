using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Caching.Redis.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Mock.Persistence;
using Infrastructure.Localization.Translation.Persistence.Mock.EntityFramework.Persistence;
using Infrastructure.Localization.Translation.Provider.Mock;
using Infrastructure.Mock.Factories;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Routing.Providers.Mock;
using Infrastructure.Security.Authentication.Mock;
using Infrastructure.Transaction.UnitOfWork.Sql.Mock;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.HR.Configuration.Mapping;
using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.AA.Mock;
using Services.Communication.Http.Broker.Department.Accounting.Mock;
using Services.Communication.Http.Broker.Department.IT.Mock;
using Services.Communication.Mq.Queue.AA.Configuration.Mock;
using Services.Communication.Mq.Queue.AA.Rabbit.Publishers.Mock;
using Services.Communication.Mq.Queue.Accounting.Configuration.Mock;
using Services.Communication.Mq.Queue.Accounting.Rabbit.Publisher.Mock;
using Services.Communication.Mq.Queue.IT.Configuration.Mock;
using Services.Communication.Mq.Queue.IT.Rabbit.Publishers.Mock;

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
