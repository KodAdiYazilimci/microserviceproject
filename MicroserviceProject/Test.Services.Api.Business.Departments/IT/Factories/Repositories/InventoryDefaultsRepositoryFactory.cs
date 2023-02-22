using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.IT.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.IT.Factories.Repositories
{
    public class InventoryDefaultsRepositoryFactory
    {
        public static InventoryDefaultsRepository GetInstance(IUnitOfWork unitOfWork)
        {
            return new InventoryDefaultsRepository(unitOfWork);
        }
    }
}
