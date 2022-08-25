using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.Accounting.Configuration.Persistence;
using Services.Api.Business.Departments.Accounting.Repositories.Sql;

using Test.Services.Api.Business.Departments.Accounting.Factories.Infrastructure;

namespace Test.Services.Api.Business.Departments.Accounting.Factories.Repositories
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
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();

                    IUnitOfWork unitOfWork = new UnitOfWork(configuration);

                    repository = new CurrencyRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
