using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.IT.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.IT.Factories.Repositories
{
    public class InventoryRepositoryFactory
    {
        public static InventoryRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new InventoryRepository(unitOfWork);
        }
    }
}
