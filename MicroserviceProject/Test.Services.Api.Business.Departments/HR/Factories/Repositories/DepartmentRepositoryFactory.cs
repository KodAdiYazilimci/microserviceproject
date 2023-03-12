using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.HR.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.HR.Factories.Repositories
{
    public class DepartmentRepositoryFactory
    {
        public static DepartmentRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new DepartmentRepository(unitOfWork);
        }
    }
}
