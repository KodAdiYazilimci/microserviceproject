using Infrastructure.Localization.Repositories;
using Infrastructure.Mock.Factories;

using Microsoft.Extensions.Configuration;

using Services.Business.Departments.Accounting.Configuration.Mapping;
using Services.Business.Departments.Accounting.Services;

using Test.Services.Business.Departments.Accounting.Factories.Infrastructure;
using Test.Services.Business.Departments.Accounting.Factories.Repositories;

namespace Test.Services.Business.Departments.Accounting.Factories.Services
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
                        unitOfWork: UnitOfWorkFactory.GetInstance(configuration),
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
                }

                return service;
            }
        }
    }
}
