using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Repositories
{
    public class DepartmentRepositoryFactory
    {
        private static DepartmentRepository departmentRepository = null;

        public static DepartmentRepository Instance
        {
            get
            {
                if (departmentRepository == null)
                {
                    AppConfigurationProvider configurationProvider = ConfigurationFactory.GetInstance();

                    departmentRepository = new DepartmentRepository(UnitOfWorkFactory.GetInstance(configurationProvider));
                }

                return departmentRepository;
            }
        }
    }
}
