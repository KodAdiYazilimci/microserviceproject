using AutoMapper;

using Infrastructure.Caching.Abstraction;
using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Caching.Redis.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Mock.Persistence;
using Infrastructure.Localization.Translation.Persistence.Mock.EntityFramework.Persistence;
using Infrastructure.Localization.Translation.Provider.Mock;
using Infrastructure.Mock.Factories;
using Infrastructure.Security.Authentication.Mock;
using Infrastructure.ServiceDiscovery.Discoverer.Mock;
using Infrastructure.ServiceDiscovery.Mock;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.HR.Configuration.Mapping;
using Services.Api.Business.Departments.HR.Configuration.Persistence;
using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Authorization.Mock;
using Services.Communication.Http.Broker.Department.AA.Mock;
using Services.Communication.Http.Broker.Department.Accounting.Mock;
using Services.Communication.Http.Broker.Department.IT.Mock;
using Services.Communication.Http.Broker.Department.Mock;
using Services.Communication.Http.Broker.Mock;
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
        public static PersonService Instance
        {
            get
            {
                IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                IMapper mapper = MappingFactory.GetInstance(new MappingProfile());
                var inMemoryCacheDataProvider = InMemoryCacheDataProviderFactory.Instance;
                IDistrubutedCacheProvider redisCacheDataProvider = CacheDataProviderFactory.GetInstance(configuration);
                var httpGetCaller = HttpGetCallerFactory.Instance;
                var httpPostCaller = HttpPostCallerFactory.Instance;
                var httpPutCaller = HttpPutCallerFactory.Instance;
                var httpDeleteCaller = HttpDeleteCallerFactory.Instance;
                var authenticatedCommunicator = AuthenticatedCommunicatorProvider.GetAuthenticatedCommunicator(
                    httpGetCaller, 
                    httpPostCaller, 
                    httpPutCaller, 
                    httpDeleteCaller);
                var authorizationCommunicator = AuthorizationCommunicatorProvider.GetAuthorizationCommunicator(
                    communicator: authenticatedCommunicator,
                    serviceDiscoverer: HttpServiceDiscovererProvider.GetServiceDiscoverer(
                        inMemoryCacheDataProvider: InMemoryCacheDataProviderFactory.Instance,
                        httpGetCaller: HttpGetCallerFactory.Instance,
                        solidServiceProvider: AppConfigSolidServiceProviderProvider.GetSolidServiceConfiguration(configuration),
                        discoveryConfiguration: AppConfigDiscoveryConfigurationProvider.GetDiscoveryConfiguration(configuration)));
                var credentialProvider = CredentialProviderFactory.GetCredentialProvider(configuration);
                var departmentCommunicator = DepartmentCommunicatorProvider.GetDepartmentCommunicator(
                    authorizationCommunicator: authorizationCommunicator,
                    inMemoryCacheDataProvider: inMemoryCacheDataProvider,
                    credentialProvider: credentialProvider,
                    communicator: authenticatedCommunicator);

                var serviceDiscoverer = HttpServiceDiscovererProvider.GetServiceDiscoverer(
                    inMemoryCacheDataProvider: InMemoryCacheDataProviderFactory.Instance,
                    httpGetCaller: HttpGetCallerFactory.Instance,
                    solidServiceProvider: AppConfigSolidServiceProviderProvider.GetSolidServiceConfiguration(configuration),
                    discoveryConfiguration: AppConfigDiscoveryConfigurationProvider.GetDiscoveryConfiguration(configuration));

                ISqlUnitOfWork unitOfWork = new UnitOfWork(configuration);

                var service = new PersonService
                    (
                        mapper: mapper,
                        aACommunicator: AACommunicatorProvider.GetAACommunicator
                        (
                            departmentCommunicator: departmentCommunicator,
                            serviceDiscoverer: serviceDiscoverer
                        ),
                        accountingCommunicator: AccountingCommunicatorProvider.GetAccountingCommunicator
                        (
                            departmentCommunicator: departmentCommunicator,
                            serviceDiscoverer: serviceDiscoverer
                        ),
                        itCommunicator: ITCommunicatorProvider.GetITCommunicator
                        (
                            departmentCommunicator: departmentCommunicator,
                            serviceDiscoverer: serviceDiscoverer
                        ),
                        AAassignInventoryToWorkerPublisher: AAAssignInventoryToWorkerPublisherProvider.GetPublisher
                        (
                            configuration: AAAssignInventoryToWorkerRabbitConfigurationProvider.GetConfiguration(configuration)
                        ),
                        ITassignInventoryToWorkerPublisher: ITAssignInventoryToWorkerPublisherProvider.GetPublisher
                        (
                            configuration: ITAssignInventoryToWorkerRabbitConfigurationProvider.GetConfiguration(configuration)
                        ),
                        createBankAccountPublisher: CreateBankAccountPublisherProvider.GetPublisher
                        (
                            rabbitConfiguration: AccountingCreateBankAccountRabbitConfigurationProvider.GetConfiguration(configuration)
                        ),
                        unitOfWork: new UnitOfWork(configuration),
                        translationProvider: TranslationProviderFactory.GetTranslationProvider
                        (
                            configuration: configuration,
                            cacheDataProvider: redisCacheDataProvider,
                            translationRepository: TranslationRepositoryFactory.GetTranslationRepository
                            (
                                translationDbContext: TranslationDbContextFactory.GetTranslationDbContext(configuration)
                            ),
                            translationHelper: TranslationHelperFactory.Instance
                        ),
                        redisCacheDataProvider: redisCacheDataProvider,
                        transactionRepository: TransactionRepositoryFactory.GetInstance(unitOfWork),
                        transactionItemRepository: TransactionItemRepositoryFactory.GetInstance(unitOfWork),
                        departmentRepository: DepartmentRepositoryFactory.GetInstance(unitOfWork),
                        personRepository: PersonRepositoryFactory.GetInstance(unitOfWork),
                        titleRepository: TitleRepositoryFactory.GetInstance(unitOfWork),
                        workerRepository: WorkerRepositoryFactory.GetInstance(unitOfWork),
                        workerRelationRepository: WorkerRelationRepositoryFactory.GetInstance(unitOfWork)
                    );

                service.TransactionIdentity = new Random().Next(int.MinValue, int.MaxValue).ToString();

                return service;
            }
        }
    }
}
