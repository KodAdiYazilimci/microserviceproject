using Infrastructure.Mock.Factories;
using Infrastructure.Transaction.UnitOfWork;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.HR.Repositories.Sql;

using Test.Services.Business.Departments.HR.Factories.Infrastructure;

namespace Test.Services.Business.Departments.HR.Factories.Repositories
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
                    IConfiguration configurationProvider = ConfigurationFactory.GetConfiguration();

                    IUnitOfWork unitOfWork = UnitOfWorkFactory.GetInstance(configurationProvider);

                    repository = new TitleRepository(unitOfWork);
                }

                return repository;
            }
        }
    }
}
