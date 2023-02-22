using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.HR.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.HR.Factories.Repositories
{
    public class DepartmentRepositoryFactory
    {
        public static DepartmentRepository GetInstance(IUnitOfWork unitOfWork)
        {
            return new DepartmentRepository(unitOfWork);
        }
    }
}
