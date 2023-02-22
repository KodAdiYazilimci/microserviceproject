using AutoMapper;

using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Caching.Redis;
using Infrastructure.Caching.Redis.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Mock.Persistence;
using Infrastructure.Localization.Translation.Persistence.Mock.EntityFramework.Persistence;
using Infrastructure.Localization.Translation.Provider.Mock;
using Infrastructure.Mock.Factories;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Routing.Providers.Mock;
using Infrastructure.Security.Authentication.Mock;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.HR.Configuration.Mapping;
using Services.Api.Business.Departments.HR.Configuration.Persistence;
using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Authorization.Mock;
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
        public static PersonService Instance
        {
            get
            {
                IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                IMapper mapper = MappingFactory.GetInstance(new MappingProfile());
                var inMemoryCacheDataProvider = InMemoryCacheDataProviderFactory.Instance;
                RedisCacheDataProvider redisCacheDataProvider = CacheDataProviderFactory.GetInstance(configuration);
                var serviceRouteRepository = ServiceRouteRepositoryFactory.GetServiceRouteRepository(configuration);
                var routeProvider = RouteProviderFactory.GetRouteProvider(
                    serviceRouteRepository: serviceRouteRepository,
                    inMemoryCacheDataProvider: inMemoryCacheDataProvider);
                var httpGetCaller = HttpGetCallerFactory.Instance;
                var httpPostCaller = HttpPostCallerFactory.Instance;
                var authorizationCommunicator = AuthorizationCommunicatorProvider.GetAuthorizationCommunicator(
                    httpGetCaller: httpGetCaller,
                    httpPostCaller: httpPostCaller,
                    routeProvider: routeProvider);
                var credentialProvider = CredentialProviderFactory.GetCredentialProvider(configuration);
                IUnitOfWork unitOfWork = new UnitOfWork(configuration);

                var service = new PersonService(
                    mapper: mapper,
                    aACommunicator: AACommunicatorProvider.GetAACommunicator(
                        authorizationCommunicator: authorizationCommunicator,
                        inMemoryCacheDataProvider: inMemoryCacheDataProvider,
                        credentialProvider: credentialProvider,
                        routeProvider: routeProvider,
                        httpGetCaller: httpGetCaller,
                        httpPostCaller: httpPostCaller),
                    accountingCommunicator: AccountingCommunicatorProvider.GetAccountingCommunicator(
                        authorizationCommunicator: authorizationCommunicator,
                        inMemoryCacheDataProvider: inMemoryCacheDataProvider,
                        credentialProvider: credentialProvider,
                        httpGetCaller: httpGetCaller,
                        httpPostCaller: httpPostCaller,
                        routeProvider: routeProvider),
                    itCommunicator: ITCommunicatorProvider.GetITCommunicator(
                        authorizationCommunicator: authorizationCommunicator,
                        inMemoryCacheDataProvider: inMemoryCacheDataProvider,
                        credentialProvider: credentialProvider,
                        httpGetCaller: httpGetCaller,
                        httpPostCaller: httpPostCaller,
                        routeProvider: routeProvider),
                    AAassignInventoryToWorkerPublisher: AAAssignInventoryToWorkerPublisherProvider.GetPublisher(
                        configuration: AAAssignInventoryToWorkerRabbitConfigurationProvider.GetConfiguration(configuration)),
                    ITassignInventoryToWorkerPublisher: ITassignInventoryToWorkerPublisherProvider.GetPublisher(
                        configuration: ITAssignInventoryToWorkerRabbitConfigurationProvider.GetConfiguration(configuration)),
                    createBankAccountPublisher: CreateBankAccountPublisherProvider.GetPublisher(
                        rabbitConfiguration: CreateBankAccountRabbitConfigurationProvider.GetConfiguration(configuration)),
                    unitOfWork: new UnitOfWork(configuration),
                    translationProvider: TranslationProviderFactory.GetTranslationProvider(
                        configuration: configuration,
                        cacheDataProvider: redisCacheDataProvider,
                        translationRepository: TranslationRepositoryFactory.GetTranslationRepository(
                            translationDbContext: TranslationDbContextFactory.GetTranslationDbContext(configuration)),
                        translationHelper: TranslationHelperFactory.Instance),
                    redisCacheDataProvider: redisCacheDataProvider,
                    transactionRepository: TransactionRepositoryFactory.GetInstance(unitOfWork),
                    transactionItemRepository: TransactionItemRepositoryFactory.GetInstance(unitOfWork),
                    departmentRepository: DepartmentRepositoryFactory.GetInstance(unitOfWork),
                    personRepository: PersonRepositoryFactory.GetInstance(unitOfWork),
                    titleRepository: TitleRepositoryFactory.GetInstance(unitOfWork),
                    workerRepository: WorkerRepositoryFactory.GetInstance(unitOfWork),
                    workerRelationRepository: WorkerRelationRepositoryFactory.GetInstance(unitOfWork));

                service.TransactionIdentity = new Random().Next(int.MinValue, int.MaxValue).ToString();

                return service;
            }
        }
    }
}
