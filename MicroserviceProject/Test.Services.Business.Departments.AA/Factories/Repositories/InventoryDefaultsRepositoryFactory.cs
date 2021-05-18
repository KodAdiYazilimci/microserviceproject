using Infrastructure.Mock.Factories;
using Infrastructure.Transaction.UnitOfWork;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.AA.Repositories.Sql;

using Test.Services.Business.Departments.AA.Factories.Infrastructure;

namespace Test.Services.Business.Departments.AA.Factories.Repositories
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
                    IConfiguration configurationProvider = ConfigurationFactory.GetConfiguration();

                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configurationProvider);

                    repository = new InventoryDefaultsRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
