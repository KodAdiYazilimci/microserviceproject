using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Caching.Redis.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Mock.Factories;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Routing.Providers.Mock;
using Infrastructure.Security.Authentication.Mock;
using Infrastructure.Transaction.UnitOfWork.Sql.Mock;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.HR.Configuration.Mapping;
using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Localization.Mock;

using Test.Services.Api.Business.Departments.HR.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.HR.Factories.Repositories;

namespace Test.Services.Api.Business.Departments.HR.Factories.Services
{
    public class DepartmentServiceFactory
    {
        private static DepartmentService departmentService = null;

        public static DepartmentService Instance
        {
            get
            {
                if (departmentService == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();

                    departmentService = new DepartmentService(
                        mapper: MappingFactory.GetInstance(new MappingProfile()),
                        unitOfWork: UnitOfWorkFactory.GetInstance(configuration),
                        redisCacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                        transactionRepository: TransactionRepositoryFactory.Instance,
                        transactionItemRepository: TransactionItemRepositoryFactory.Instance,
                        departmentRepository: DepartmentRepositoryFactory.Instance,
                        localizationCommunicator: LocalizationCommunicatorProvider.GetLocalizationCommunicator(
                            routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                            serviceCommunicator: ServiceCommunicatorFactory.GetServiceCommunicator(
                                cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                                credentialProvider: CredentialProviderFactory.GetCredentialProvider(configuration),
                                routeNameProvider: RouteNameProviderFactory.GetRouteNameProvider(configuration),
                                serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(configuration))));
                }

                return departmentService;
            }
        }
    }
}
