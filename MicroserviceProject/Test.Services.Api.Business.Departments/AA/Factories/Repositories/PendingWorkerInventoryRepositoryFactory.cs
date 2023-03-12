using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.AA.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.AA.Factories.Repositories
{
    public class PendingWorkerInventoryRepositoryFactory
    {
        public static PendingWorkerInventoryRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new PendingWorkerInventoryRepository(unitOfWork);
        }
    }
}
