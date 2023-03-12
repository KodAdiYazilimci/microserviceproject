using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.Accounting.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.Accounting.Factories.Repositories
{
    public class SalaryPaymentRepositoryFactory
    {
        public static SalaryPaymentRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new SalaryPaymentRepository(unitOfWork);
        }
    }
}
