using MicroserviceProject.Services.Business.Departments.HR.Configuration.Mapping;
using MicroserviceProject.Services.Business.Departments.HR.Services;
using MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Repositories;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Repositories;
using MicroserviceProject.Test.Services.Factories;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Services
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
                        cacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                        transactionRepository: TransactionRepositoryFactory.Instance,
                        transactionItemRepository: TransactionItemRepositoryFactory.Instance,
                        departmentRepository: DepartmentRepositoryFactory.Instance);
                }

                return departmentService;
            }
        }
    }
}
