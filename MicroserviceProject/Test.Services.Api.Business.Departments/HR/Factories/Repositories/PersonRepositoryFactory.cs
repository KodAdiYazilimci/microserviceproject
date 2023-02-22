using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.HR.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.HR.Factories.Repositories
{
    public class PersonRepositoryFactory
    {
        public static PersonRepository GetInstance(IUnitOfWork unitOfWork)
        {
            return new PersonRepository(unitOfWork);
        }
    }
}
