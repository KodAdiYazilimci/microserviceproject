using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure;
using MicroserviceProject.Services.UnitOfWork;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Repositories
{
    public class WorkerRepositoryFactory
    {
        private static WorkerRepository repository = null;

        public static WorkerRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configurationProvider = ConfigurationFactory.GetConfiguration();

                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configurationProvider);

                    repository = new WorkerRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
