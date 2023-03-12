using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.IT.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.IT.Factories.Repositories
{
    public class PendingWorkerInventoryRepositoryFactory
    {
        public static PendingWorkerInventoryRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new PendingWorkerInventoryRepository(unitOfWork);
        }
    }
}
