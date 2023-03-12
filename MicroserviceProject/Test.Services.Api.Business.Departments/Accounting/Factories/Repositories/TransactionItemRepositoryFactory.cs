using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.Accounting.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.Accounting.Factories.Repositories
{
    public class TransactionItemRepositoryFactory
    {
        public static TransactionItemRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new TransactionItemRepository(unitOfWork);
        }
    }
}
