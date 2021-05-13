using Infrastructure.Mock.Factories;
using Infrastructure.Transaction.UnitOfWork;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.HR.Repositories.Sql;

using Test.Services.Business.Departments.HR.Factories.Infrastructure;

namespace Test.Services.Business.Departments.HR.Factories.Repositories
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
