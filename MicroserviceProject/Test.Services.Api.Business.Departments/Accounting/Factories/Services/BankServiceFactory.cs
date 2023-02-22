using AutoMapper;

using Infrastructure.Caching.Redis;
using Infrastructure.Caching.Redis.Mock;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Repositories;
using Infrastructure.Localization.Translation.Persistence.Mock.EntityFramework.Persistence;
using Infrastructure.Localization.Translation.Provider.Mock;
using Infrastructure.Mock.Factories;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.Accounting.Configuration.Mapping;
using Services.Api.Business.Departments.Accounting.Configuration.Persistence;
using Services.Api.Business.Departments.Accounting.Services;

using Test.Services.Api.Business.Departments.Accounting.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.Accounting.Factories.Repositories;

namespace Test.Services.Api.Business.Departments.Accounting.Factories.Services
{
    public class BankServiceFactory
    {
        public static BankService Instance
        {
            get
            {
                IConfiguration configuration = ConfigurationFactory.GetConfiguration();
                IMapper mapper = MappingFactory.GetInstance(new MappingProfile());
                RedisCacheDataProvider redisCacheDataProvider = CacheDataProviderFactory.GetInstance(configuration);
                IUnitOfWork unitOfWork = new UnitOfWork(configuration);

                var service = new BankService(
                    mapper: mapper,
                    unitOfWork: new UnitOfWork(configuration),
                    translationProvider: TranslationProviderFactory.GetTranslationProvider(
                        configuration: configuration,
                        cacheDataProvider: redisCacheDataProvider,
                        translationRepository: new TranslationRepository(TranslationDbContextFactory.GetTranslationDbContext(configuration)),
                        translationHelper: TranslationHelperFactory.Instance),
                    redisCacheDataProvider: redisCacheDataProvider,
                    transactionItemRepository: TransactionItemRepositoryFactory.GetInstance(unitOfWork),
                    transactionRepository: TransactionRepositoryFactory.GetInstance(unitOfWork),
                    bankAccountRepository: BankAccountRepositoryFactory.GetInstance(unitOfWork),
                    currencyRepository: CurrencyRepositoryFactory.GetInstance(unitOfWork),
                    salaryPaymentRepository: SalaryPaymentRepositoryFactory.GetInstance(unitOfWork));

                service.TransactionIdentity = new Random().Next(int.MinValue, int.MaxValue).ToString();

                return service;
            }
        }
    }
}
