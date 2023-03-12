using AutoMapper;

using Infrastructure.Caching.Redis;
using Infrastructure.Caching.Redis.Mock;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Repositories;
using Infrastructure.Localization.Translation.Persistence.Mock.EntityFramework.Persistence;
using Infrastructure.Localization.Translation.Provider.Mock;
using Infrastructure.Mock.Factories;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.HR.Configuration.Mapping;
using Services.Api.Business.Departments.HR.Configuration.Persistence;
using Services.Api.Business.Departments.HR.Services;

using Test.Services.Api.Business.Departments.HR.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.HR.Factories.Repositories;

namespace Test.Services.Api.Business.Departments.HR.Factories.Services
{
    public class DepartmentServiceFactory
    {
        public static DepartmentService Instance
        {
            get
            {
                IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                IMapper mapper = MappingFactory.GetInstance(new MappingProfile());
                RedisCacheDataProvider redisCacheDataProvider = CacheDataProviderFactory.GetInstance(configuration);
                IUnitOfWork unitOfWork = new UnitOfWork(configuration);

                var service = new DepartmentService
                    (
                        mapper: mapper,
                        unitOfWork: new UnitOfWork(configuration),
                        redisCacheDataProvider: redisCacheDataProvider,
                        translationProvider: TranslationProviderFactory.GetTranslationProvider
                        (
                            configuration: configuration,
                            cacheDataProvider: redisCacheDataProvider,
                            translationRepository: new EfTranslationRepository
                            (
                                translationDbContext: TranslationDbContextFactory.GetTranslationDbContext(configuration)
                            ),
                            translationHelper: TranslationHelperFactory.Instance
                        ),
                        transactionRepository: TransactionRepositoryFactory.GetInstance(unitOfWork),
                        transactionItemRepository: TransactionItemRepositoryFactory.GetInstance(unitOfWork),
                        departmentRepository: DepartmentRepositoryFactory.GetInstance(unitOfWork)
                    );

                service.TransactionIdentity = new Random().Next(int.MinValue, int.MaxValue).ToString();

                return service;
            }
        }
    }
}
