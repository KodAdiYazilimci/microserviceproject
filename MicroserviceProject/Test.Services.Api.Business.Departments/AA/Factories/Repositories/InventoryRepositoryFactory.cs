using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.AA.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.AA.Factories.Repositories
{
    public class InventoryRepositoryFactory
    {
        public static InventoryRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new InventoryRepository(unitOfWork);
        }
    }
}
