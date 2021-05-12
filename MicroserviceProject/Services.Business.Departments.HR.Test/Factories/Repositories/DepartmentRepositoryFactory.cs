using MicroserviceProject.Infrastructure.Transaction.UnitOfWork;
using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Repositories
{
    public class DepartmentRepositoryFactory
    {
        private static DepartmentRepository repository = null;

        public static DepartmentRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configurationProvider = ConfigurationFactory.GetConfiguration();

                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configurationProvider);

                    repository = new DepartmentRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
