using Infrastructure.Transaction.UnitOfWork.Sql;
using Infrastructure.Transaction.UnitOfWork.Sql.Mock;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.Accounting.Repositories.Sql;

using Test.Services.Api.Business.Departments.Accounting.Factories.Infrastructure;

namespace Test.Services.Api.Business.Departments.Accounting.Factories.Repositories
{
    public class BankAccountRepositoryFactory
    {
        private static BankAccountRepository repository;

        public static BankAccountRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configurationProvider = ConfigurationFactory.GetConfiguration();

                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configurationProvider);

                    repository = new BankAccountRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
