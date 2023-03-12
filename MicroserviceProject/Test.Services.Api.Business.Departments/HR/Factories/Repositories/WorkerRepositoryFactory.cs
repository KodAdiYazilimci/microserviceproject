using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.HR.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.HR.Factories.Repositories
{
    public class WorkerRepositoryFactory
    {
        public static WorkerRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new WorkerRepository(unitOfWork);
        }
    }
}
