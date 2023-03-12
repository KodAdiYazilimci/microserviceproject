using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.AA.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.AA.Factories.Repositories
{
    public class WorkerInventoryRepositoryFactory
    {
        public static WorkerInventoryRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new WorkerInventoryRepository(unitOfWork);
        }
    }
}
