using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Repositories
{
    public class TransactionRepositoryFactory
    {
        private static TransactionRepository transactionRepository = null;

        public static TransactionRepository Instance
        {
            get
            {
                if (transactionRepository == null)
                {
                    AppConfigurationProvider configurationProvider = ConfigurationFactory.GetInstance();

                    transactionRepository = new TransactionRepository(UnitOfWorkFactory.GetInstance(configurationProvider));
                }

                return transactionRepository;
            }
        }
    }
}
