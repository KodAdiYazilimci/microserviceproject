using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure;
using MicroserviceProject.Services.UnitOfWork;

using Microsoft.Extensions.Configuration;

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
                    IConfiguration configurationProvider = ConfigurationFactory.GetConfiguration();
                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configurationProvider);

                    transactionRepository = new TransactionRepository(unitOfWork);
                }

                return transactionRepository;
            }
        }
    }
}
