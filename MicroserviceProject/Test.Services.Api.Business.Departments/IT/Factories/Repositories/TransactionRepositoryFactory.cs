using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.IT.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.IT.Factories.Repositories
{
    public class TransactionRepositoryFactory
    {
        public static TransactionRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new TransactionRepository(unitOfWork);
        }
    }
}
