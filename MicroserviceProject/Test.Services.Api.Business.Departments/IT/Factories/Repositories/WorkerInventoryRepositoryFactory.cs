using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.IT.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.IT.Factories.Repositories
{
    public class WorkerInventoryRepositoryFactory
    {
        public static WorkerInventoryRepository GetInstance(IUnitOfWork unitOfWork)
        {
            return new WorkerInventoryRepository(unitOfWork);
        }
    }
}
