using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.IT.Configuration.Persistence;
using Services.Api.Business.Departments.IT.Repositories.Sql;

using Test.Services.Api.Business.Departments.IT.Factories.Infrastructure;

namespace Test.Services.Api.Business.Departments.IT.Factories.Repositories
{
    public class InventoryDefaultsRepositoryFactory
    {
        private static InventoryDefaultsRepository repository;

        public static InventoryDefaultsRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                    IUnitOfWork unitOfWork = new UnitOfWork(configuration);
                    repository = new InventoryDefaultsRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
