using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.HR.Configuration.Persistence;
using Services.Api.Business.Departments.HR.Repositories.Sql;

using Test.Services.Api.Business.Departments.HR.Factories.Infrastructure;

namespace Test.Services.Api.Business.Departments.HR.Factories.Repositories
{
    public class TitleRepositoryFactory
    {
        private static TitleRepository repository = null;

        public static TitleRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                    IUnitOfWork unitOfWork = new UnitOfWork(configuration);
                    repository = new TitleRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
