using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.Accounting.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.Accounting.Factories.Repositories
{
    public class BankAccountRepositoryFactory
    {
        public static BankAccountRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new BankAccountRepository(unitOfWork);
        }
    }
}
