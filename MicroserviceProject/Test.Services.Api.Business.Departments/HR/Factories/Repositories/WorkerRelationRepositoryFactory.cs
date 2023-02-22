using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.HR.Repositories.Sql;

namespace Test.Services.Api.Business.Departments.HR.Factories.Repositories
{
    public class WorkerRelationRepositoryFactory
    {
        public static WorkerRelationRepository GetInstance(IUnitOfWork unitOfWork)
        {
            return new WorkerRelationRepository(unitOfWork);
        }
    }
}
