using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.HR.Configuration.Persistence;
using Services.Api.Business.Departments.HR.Repositories.Sql;

using Test.Services.Api.Business.Departments.HR.Factories.Infrastructure;

namespace Test.Services.Api.Business.Departments.HR.Factories.Repositories
{
    public class PersonRepositoryFactory
    {
        private static PersonRepository repository = null;

        public static PersonRepository Instance
        {
            get
            {
                if (repository == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                    IUnitOfWork unitOfWork = new UnitOfWork(configuration);
                    repository = new PersonRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
