using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.Accounting.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.Accounting.Factories.Repositories
{
    public class CurrencyRepositoryFactory
    {
        public static CurrencyRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new CurrencyRepository(unitOfWork);
        }
    }
}
