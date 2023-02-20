using Infrastructure.Caching.Redis.Mock;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Repositories;
using Infrastructure.Localization.Translation.Persistence.Mock.EntityFramework.Persistence;
using Infrastructure.Localization.Translation.Provider.Mock;
using Infrastructure.Mock.Factories;

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
        private static BankService service;

        public static BankService Instance
        {
            get
            {
                if (service == null)
                {
                    IConfiguration configuration = ConfigurationFactory.GetConfiguration();

                    service = new BankService(
                        mapper: MappingFactory.GetInstance(new MappingProfile()),
                        unitOfWork: new UnitOfWork(configuration),
                        translationProvider: TranslationProviderFactory.GetTranslationProvider(
                            configuration: configuration,
                            cacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                            translationRepository: new TranslationRepository(TranslationDbContextFactory.GetTranslationDbContext(configuration)),
                            translationHelper: TranslationHelperFactory.Instance),
                        redisCacheDataProvider: CacheDataProviderFactory.GetInstance(configuration),
                        transactionItemRepository: TransactionItemRepositoryFactory.Instance,
                        transactionRepository: TransactionRepositoryFactory.Instance,
                        bankAccountRepository: BankAccountRepositoryFactory.Instance,
                        currencyRepository: CurrencyRepositoryFactory.Instance,
                        salaryPaymentRepository: SalaryPaymentRepositoryFactory.Instance);

                    service.TransactionIdentity = new Random().Next(int.MinValue, int.MaxValue).ToString();
                }

                return service;
            }
        }
    }
}
