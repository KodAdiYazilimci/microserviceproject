using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.AA.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.AA.Factories.Repositories
{
    public class InventoryDefaultsRepositoryFactory
    {
        public static InventoryDefaultsRepository GetInstance(ISqlUnitOfWork unitOfWork)
        {
            return new InventoryDefaultsRepository(unitOfWork);
        }
    }
}
