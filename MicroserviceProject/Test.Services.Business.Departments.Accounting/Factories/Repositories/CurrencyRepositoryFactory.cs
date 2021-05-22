using Infrastructure.Mock.Factories;
using Infrastructure.Transaction.UnitOfWork;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.Accounting.Repositories.Sql;

using Test.Services.Business.Departments.Accounting.Factories.Infrastructure;

namespace Test.Services.Business.Departments.Accounting.Factories.Repositories
{
    public class CurrencyRepositoryFactory
    {
        private static CurrencyRepository repository;

        public static CurrencyRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configurationProvider = ConfigurationFactory.GetConfiguration();

                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configurationProvider);

                    repository = new CurrencyRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
