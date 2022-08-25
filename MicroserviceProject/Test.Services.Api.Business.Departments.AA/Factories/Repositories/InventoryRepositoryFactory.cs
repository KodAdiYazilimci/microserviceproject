using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.AA.Configuration.Persistence;
using Services.Api.Business.Departments.AA.Repositories.Sql;

using Test.Services.Api.Business.Departments.AA.Factories.Infrastructure;

namespace Test.Services.Api.Business.Departments.AA.Factories.Repositories
{
    public class InventoryRepositoryFactory
    {
        private static InventoryRepository repository;

        public static InventoryRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                    IUnitOfWork unitOfWork = new UnitOfWork(configuration);
                    repository = new InventoryRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
