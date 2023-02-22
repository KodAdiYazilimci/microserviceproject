using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.IT.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.IT.Factories.Repositories
{
    public class TransactionItemRepositoryFactory
    {
        public static TransactionItemRepository GetInstance(IUnitOfWork unitOfWork)
        {
            return new TransactionItemRepository(unitOfWork);
        }
    }
}
