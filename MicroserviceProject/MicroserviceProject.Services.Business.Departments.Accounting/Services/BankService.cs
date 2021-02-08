using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Services.Business.Departments.Accounting.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.Accounting.Repositories.Sql;
using MicroserviceProject.Services.Business.Model.Department.Accounting;
using MicroserviceProject.Services.Business.Util.UnitOfWork;

using System;
using System.Collections.Generic;
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
