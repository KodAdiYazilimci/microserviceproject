using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure;
using MicroserviceProject.Services.UnitOfWork;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Repositories
{
    public class TransactionItemRepositoryFactory
    {
        private static TransactionItemRepository transactionItemRepository = null;

        public static TransactionItemRepository Instance
        {
            get
            {
                if (transactionItemRepository == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configuration);

                    transactionItemRepository = new TransactionItemRepository(unitOfWork);
                }

                return transactionItemRepository;
            }
        }
    }
}
