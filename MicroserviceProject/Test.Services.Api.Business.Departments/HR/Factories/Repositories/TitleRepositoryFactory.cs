using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.HR.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.HR.Factories.Repositories
{
    public class TitleRepositoryFactory
    {
        public static TitleRepository GetInstance(IUnitOfWork unitOfWork)
        {
            return new TitleRepository(unitOfWork);
        }
    }
}
