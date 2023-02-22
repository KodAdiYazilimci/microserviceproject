using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.Accounting.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.Accounting.Factories.Repositories
{
    public class TransactionRepositoryFactory
    {
        public static TransactionRepository GetInstance(IUnitOfWork unitOfWork)
        {
            return new TransactionRepository(unitOfWork);
        }
    }
}
