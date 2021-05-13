using Infrastructure.Localization.Repositories;
using Infrastructure.Mock.Factories;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.HR.Configuration.Mapping;
using Services.Business.Departments.HR.Services;

using Test.Services.Business.Departments.HR.Factories.Infrastructure;
using Test.Services.Business.Departments.HR.Factories.Repositories;

namespace Test.Services.Business.Departments.HR.Factories.Services
{
    public class DepartmentServiceFactory
    {
        private static DepartmentService departmentService = null;

        public static DepartmentService Instance
        {
            get
            {
                if (departmentService == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();

                    departmentService = new DepartmentService(
                        mapper: MappingFactory.GetInstance(new MappingProfile()),
                        unitOfWork: UnitOfWorkFactory.GetInstance(configuration),
                        cacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                        translationProvider: TranslationProviderFactory.GetTranslationProvider(
                            configuration: configuration,
                            cacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                            translationRepository: new TranslationRepository(TranslationDbContextFactory.GetTranslationDbContext(configuration)),
                            translationHelper: TranslationHelperFactory.Instance),
                        transactionRepository: TransactionRepositoryFactory.Instance,
                        transactionItemRepository: TransactionItemRepositoryFactory.Instance,
                        departmentRepository: DepartmentRepositoryFactory.Instance);
                }

                return departmentService;
            }
        }
    }
}
