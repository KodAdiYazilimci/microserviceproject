using Infrastructure.Mock.Factories;
using Infrastructure.Transaction.UnitOfWork;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.IT.Repositories.Sql;

using Test.Services.Business.Departments.IT.Factories.Infrastructure;

namespace Test.Services.Business.Departments.IT.Factories.Repositories
{
    public class WorkerInventoryRepositoryFactory
    {
        private static WorkerInventoryRepository repository;

        public static WorkerInventoryRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configurationProvider = ConfigurationFactory.GetConfiguration();

                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configurationProvider);

                    repository = new WorkerInventoryRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
