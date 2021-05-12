using MicroserviceProject.Services.Business.Departments.HR.Configuration.Mapping;
using MicroserviceProject.Services.Business.Departments.HR.Services;
using MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Repositories;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Repositories;
using MicroserviceProject.Test.Services.Factories;
using MicroserviceProject.Test.Services.Providers.Publisher;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Services
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
