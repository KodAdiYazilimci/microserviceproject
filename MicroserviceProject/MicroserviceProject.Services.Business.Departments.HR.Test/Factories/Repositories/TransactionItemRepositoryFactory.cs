using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure;

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
                    AppConfigurationProvider configurationProvider = ConfigurationFactory.GetInstance();

                    transactionItemRepository = new TransactionItemRepository(UnitOfWorkFactory.GetInstance(configurationProvider));
                }

                return transactionItemRepository;
            }
        }
    }
}
