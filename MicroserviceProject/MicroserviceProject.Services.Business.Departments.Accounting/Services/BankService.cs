using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Services.Business.Departments.Accounting.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.Accounting.Repositories.Sql;
using MicroserviceProject.Services.Business.Model.Department.Accounting;
using MicroserviceProject.Services.Business.Util.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Accounting.Services
{
    /// <summary>
    /// Banka hesapları iş mantığı sınıfı
    /// </summary>
    public class BankService : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Önbelleğe alınan para birimlerinin önbellekteki adı
        /// </summary>
        private const string CACHED_CURRENCIES_KEY = "MicroserviceProject.Services.Business.Departments.Accounting.Currencies";

        /// <summary>
        /// Rediste tutulan önbellek yönetimini sağlayan sınıf
        /// </summary>
        private readonly CacheDataProvider _cacheDataProvider;

        /// <summary>
        /// Mapping işlemleri için mapper nesnesi
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Banka hesapları repository sınıfı
        /// </summary>
        private readonly BankAccountRepository _bankAccountRepository;

        /// <summary>
        /// Para birimleri repository sınıfı>
        /// </summary>
        private readonly CurrencyRepository _currencyRepository;

        /// <summary>
        /// Maaş ödemeleri repository sınıfı
        /// </summary>
        private readonly SalaryPaymentRepository _salaryPaymentRepository;

        /// <summary>
        /// Banka hesapları iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="cacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="bankAccountRepository">Banka hesapları repository sınıfı</param>
        /// <param name="currencyRepository">Para birimleri repository sınıfı></param>
        /// <param name="salaryPaymentRepository">Maaş ödemeleri repository sınıfı</param>
        public BankService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            CacheDataProvider cacheDataProvider,
            BankAccountRepository bankAccountRepository,
            CurrencyRepository currencyRepository,
            SalaryPaymentRepository salaryPaymentRepository)
        {
            _mapper = mapper;
            _cacheDataProvider = cacheDataProvider;
            _unitOfWork = unitOfWork;

            _bankAccountRepository = bankAccountRepository;
            _currencyRepository = currencyRepository;
            _salaryPaymentRepository = salaryPaymentRepository;
        }

        /// <summary>
        /// Bir çalışanın maaş hesabı listesini verir
        /// </summary>
        /// <param name="workerId">Çalışanın Id si</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<BankAccountModel>> GetBankAccounts(int workerId, CancellationToken cancellationToken)
        {
            List<BankAccountEntity> bankAccounts =
                await _bankAccountRepository.GetBankAccountsAsync(workerId, cancellationToken);

            List<BankAccountModel> mappedBankAccounts =
                _mapper.Map<List<BankAccountEntity>, List<BankAccountModel>>(bankAccounts);

            return mappedBankAccounts;
        }

        /// <summary>
        /// Yeni maaş hesabı oluşturur
        /// </summary>
        /// <param name="bankAccount">Oluşturulacak banka hesabı nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateBankAccountAsync(BankAccountModel bankAccount, CancellationToken cancellationToken)
        {
            BankAccountEntity mappedBankAccount = _mapper.Map<BankAccountModel, BankAccountEntity>(bankAccount);

            int createdBankAccountId = await _bankAccountRepository.CreateAsync(mappedBankAccount, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return createdBankAccountId;
        }

        /// <summary>
        /// Para birimlerinin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<CurrencyModel>> GetCurrenciesAsync(CancellationToken cancellationToken)
        {
            if (_cacheDataProvider.TryGetValue(CACHED_CURRENCIES_KEY, out List<CurrencyModel> cureencies)
                &&
                cureencies != null && cureencies.Any())
            {
                return cureencies;
            }

            List<CurrencyEntity> currencies = await _currencyRepository.GetListAsync(cancellationToken);

            List<CurrencyModel> mappedDepartments =
                _mapper.Map<List<CurrencyEntity>, List<CurrencyModel>>(currencies);

            _cacheDataProvider.Set(CACHED_CURRENCIES_KEY, mappedDepartments);

            return mappedDepartments;
        }

        /// <summary>
        /// Yeni para birimi oluşturur
        /// </summary>
        /// <param name="currency">Oluşturulacak para birimi nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateCurrencyAsync(CurrencyModel currency, CancellationToken cancellationToken)
        {
            CurrencyEntity mappedCurrency = _mapper.Map<CurrencyModel, CurrencyEntity>(currency);

            int createdCurrencyId = await _currencyRepository.CreateAsync(mappedCurrency, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            currency.Id = createdCurrencyId;

            if (_cacheDataProvider.TryGetValue(CACHED_CURRENCIES_KEY, out List<CurrencyModel> cachedCurrencies))
            {
                cachedCurrencies.Add(currency);
                _cacheDataProvider.Set(CACHED_CURRENCIES_KEY, cachedCurrencies);
            }
            else
            {
                List<CurrencyModel> currencies = await GetCurrenciesAsync(cancellationToken);

                currencies.Add(currency);

                _cacheDataProvider.Set(CACHED_CURRENCIES_KEY, currencies);
            }

            return createdCurrencyId;
        }

        /// <summary>
        /// Bir çalışanın maaş ödemelerini verir
        /// </summary>
        /// <param name="workerId">Çalışanın Id si</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<SalaryPaymentModel>> GetSalaryPaymentsOfWorkerAsync(int workerId, CancellationToken cancellationToken)
        {
            List<SalaryPaymentEntity> bankAccounts =
           await _salaryPaymentRepository.GetSalaryPaymentsOfWorkerAsync(workerId, cancellationToken);

            List<SalaryPaymentModel> mappedSalaryPayments =
                _mapper.Map<List<SalaryPaymentEntity>, List<SalaryPaymentModel>>(bankAccounts);

            return mappedSalaryPayments;
        }

        /// <summary>
        /// Yeni bir maaş ödemesi oluşturur
        /// </summary>
        /// <param name="salaryPayment">Oluşturulacak maaş ödemesinin nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateSalaryPaymentAsync(SalaryPaymentModel salaryPayment, CancellationToken cancellationToken)
        {
            SalaryPaymentEntity mappedBankAccount = _mapper.Map<SalaryPaymentModel, SalaryPaymentEntity>(salaryPayment);

            int createdSalaryPaymentId = await _salaryPaymentRepository.CreateAsync(mappedBankAccount, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return createdSalaryPaymentId;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    _cacheDataProvider.Dispose();
                    _unitOfWork.Dispose();
                }

                disposed = true;
            }
        }
    }
}
