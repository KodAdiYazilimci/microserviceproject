using Infrastructure.Mock.Factories;
using Infrastructure.Transaction.UnitOfWork;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.AA.Repositories.Sql;

using Test.Services.Business.Departments.AA.Factories.Infrastructure;

namespace Test.Services.Business.Departments.AA.Factories.Repositories
{
    public class TransactionItemRepositoryFactory
    {
        private static TransactionItemRepository repository = null;

        public static TransactionItemRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configuration);

                    repository = new TransactionItemRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
