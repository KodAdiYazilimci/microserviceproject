using Infrastructure.Mock.Factories;
using Infrastructure.Transaction.UnitOfWork;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.AA.Repositories.Sql;

using Test.Services.Business.Departments.AA.Factories.Infrastructure;

namespace Test.Services.Business.Departments.AA.Factories.Repositories
{
    public class PendingWorkerInventoryRepositoryFactory
    {
        private static PendingWorkerInventoryRepository repository;

        public static PendingWorkerInventoryRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configurationProvider = ConfigurationFactory.GetConfiguration();

                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configurationProvider);

                    repository = new PendingWorkerInventoryRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
