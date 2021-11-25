using Infrastructure.Transaction.UnitOfWork.Sql;
using Infrastructure.Transaction.UnitOfWork.Sql.Mock;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.HR.Repositories.Sql;

using Test.Services.Api.Business.Departments.HR.Factories.Infrastructure;

namespace Test.Services.Api.Business.Departments.HR.Factories.Repositories
{
    public class WorkerRelationRepositoryFactory
    {
        private static WorkerRelationRepository repository = null;

        public static WorkerRelationRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configurationProvider = ConfigurationFactory.GetConfiguration();

                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configurationProvider);

                    repository = new WorkerRelationRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
