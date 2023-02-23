using AutoMapper;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Localization.Translation.Provider;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.Accounting.Entities.Sql;
using Services.Api.Business.Departments.Accounting.Repositories.Sql;
using Services.Communication.Http.Broker.Department.Accounting.Models;
using Services.Logging.Aspect.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Services
{
    /// <summary>
    /// Banka hesapları iş mantığı sınıfı
    /// </summary>
    public class BankService : BaseService, IRollbackableAsync<int>, IDisposable, IDisposableInjections
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Api.Business.Departments.Accounting.Services.BankService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Api.Business.Departments.Accounting";

        /// <summary>
        /// Önbelleğe alınan para birimlerinin önbellekteki adı
        /// </summary>
        private const string CACHED_CURRENCIES_KEY = "Services.Api.Business.Departments.Accounting.Currencies";

        /// <summary>
        /// Rediste tutulan önbellek yönetimini sağlayan sınıf
        /// </summary>
        private readonly RedisCacheDataProvider _redisCacheDataProvider;

        /// <summary>
        /// Mapping işlemleri için mapper nesnesi
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// İşlem tablosu için repository sınıfı
        /// </summary>
        private readonly TransactionRepository _transactionRepository;

        /// <summary>
        /// İşlem öğesi tablosu için repository sınıfı
        /// </summary>
        private readonly TransactionItemRepository _transactionItemRepository;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

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
        /// <param name="translationProvider">Dil çeviri sağlayıcısı sınıf</param>
        /// <param name="redisCacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="bankAccountRepository">Banka hesapları repository sınıfı</param>
        /// <param name="currencyRepository">Para birimleri repository sınıfı></param>
        /// <param name="salaryPaymentRepository">Maaş ödemeleri repository sınıfı</param>
        public BankService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            TranslationProvider translationProvider,
            RedisCacheDataProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            BankAccountRepository bankAccountRepository,
            CurrencyRepository currencyRepository,
            SalaryPaymentRepository salaryPaymentRepository)
        {
            _mapper = mapper;
            _redisCacheDataProvider = redisCacheDataProvider;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _bankAccountRepository = bankAccountRepository;
            _currencyRepository = currencyRepository;
            _salaryPaymentRepository = salaryPaymentRepository;
        }

        /// <summary>
        /// Bir çalışanın maaş hesabı listesini verir
        /// </summary>
        /// <param name="workerId">Çalışanın Id si</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(GetBankAccounts))]
        [LogAfterRuntimeAttr(nameof(GetBankAccounts))]
        public async Task<List<AccountingBankAccountModel>> GetBankAccounts(int workerId, CancellationTokenSource cancellationTokenSource)
        {
            List<BankAccountEntity> bankAccounts =
                await _bankAccountRepository.GetBankAccountsAsync(workerId, cancellationTokenSource);

            List<AccountingBankAccountModel> mappedBankAccounts =
                _mapper.Map<List<BankAccountEntity>, List<AccountingBankAccountModel>>(bankAccounts);

            return mappedBankAccounts;
        }

        /// <summary>
        /// Yeni maaş hesabı oluşturur
        /// </summary>
        /// <param name="bankAccount">Oluşturulacak banka hesabı nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(CreateBankAccountAsync))]
        [LogAfterRuntimeAttr(nameof(CreateBankAccountAsync))]
        public async Task<int> CreateBankAccountAsync(AccountingBankAccountModel bankAccount, CancellationTokenSource cancellationTokenSource)
        {
            BankAccountEntity mappedBankAccount = _mapper.Map<AccountingBankAccountModel, BankAccountEntity>(bankAccount);

            int createdBankAccountId = await _bankAccountRepository.CreateAsync(mappedBankAccount, cancellationTokenSource);

            await CreateCheckpointAsync(
                rollback: new RollbackModel()
                {
                    TransactionIdentity = TransactionIdentity,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = TransactionType.Insert,
                    RollbackItems = new List<RollbackItemModel>
                    {
                         new RollbackItemModel
                         {
                             Identity = createdBankAccountId,
                             DataSet = BankAccountRepository.TABLE_NAME,
                             RollbackType = RollbackType.Delete
                         }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            return createdBankAccountId;
        }

        /// <summary>
        /// Para birimlerinin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(GetCurrenciesAsync))]
        [LogAfterRuntimeAttr(nameof(GetCurrenciesAsync))]
        public async Task<List<AccountingCurrencyModel>> GetCurrenciesAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_CURRENCIES_KEY, out List<AccountingCurrencyModel> cureencies)
                &&
                cureencies != null && cureencies.Any())
            {
                return cureencies;
            }

            List<CurrencyEntity> currencies = await _currencyRepository.GetListAsync(cancellationTokenSource);

            List<AccountingCurrencyModel> mappedDepartments =
                _mapper.Map<List<CurrencyEntity>, List<AccountingCurrencyModel>>(currencies);

            _redisCacheDataProvider.Set(CACHED_CURRENCIES_KEY, mappedDepartments);

            return mappedDepartments;
        }

        /// <summary>
        /// Yeni para birimi oluşturur
        /// </summary>
        /// <param name="currency">Oluşturulacak para birimi nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(CreateCurrencyAsync))]
        [LogAfterRuntimeAttr(nameof(CreateCurrencyAsync))]
        public async Task<int> CreateCurrencyAsync(AccountingCurrencyModel currency, CancellationTokenSource cancellationTokenSource)
        {
            CurrencyEntity mappedCurrency = _mapper.Map<AccountingCurrencyModel, CurrencyEntity>(currency);

            int createdCurrencyId = await _currencyRepository.CreateAsync(mappedCurrency, cancellationTokenSource);

            await CreateCheckpointAsync(
                rollback: new RollbackModel()
                {
                    TransactionDate = DateTime.UtcNow,
                    TransactionIdentity = TransactionIdentity,
                    TransactionType = TransactionType.Insert,
                    RollbackItems = new List<RollbackItemModel>
                    {
                        new RollbackItemModel
                        {
                             Identity = createdCurrencyId,
                             DataSet = CurrencyRepository.TABLE_NAME,
                             RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            currency.Id = createdCurrencyId;

            if (_redisCacheDataProvider.TryGetValue(CACHED_CURRENCIES_KEY, out List<AccountingCurrencyModel> cachedCurrencies) && cachedCurrencies != null)
            {
                cachedCurrencies.Add(currency);

                _redisCacheDataProvider.Set(CACHED_CURRENCIES_KEY, cachedCurrencies);
            }

            return createdCurrencyId;
        }

        /// <summary>
        /// Bir çalışanın maaş ödemelerini verir
        /// </summary>
        /// <param name="workerId">Çalışanın Id si</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(GetSalaryPaymentsOfWorkerAsync))]
        [LogAfterRuntimeAttr(nameof(GetSalaryPaymentsOfWorkerAsync))]
        public async Task<List<AccountingSalaryPaymentModel>> GetSalaryPaymentsOfWorkerAsync(int workerId, CancellationTokenSource cancellationTokenSource)
        {
            List<SalaryPaymentEntity> bankAccounts =
           await _salaryPaymentRepository.GetSalaryPaymentsOfWorkerAsync(workerId, cancellationTokenSource);

            List<AccountingSalaryPaymentModel> mappedSalaryPayments =
                _mapper.Map<List<SalaryPaymentEntity>, List<AccountingSalaryPaymentModel>>(bankAccounts);

            return mappedSalaryPayments;
        }

        /// <summary>
        /// Yeni bir maaş ödemesi oluşturur
        /// </summary>
        /// <param name="salaryPayment">Oluşturulacak maaş ödemesinin nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(CreateSalaryPaymentAsync))]
        [LogAfterRuntimeAttr(nameof(CreateSalaryPaymentAsync))]
        public async Task<int> CreateSalaryPaymentAsync(AccountingSalaryPaymentModel salaryPayment, CancellationTokenSource cancellationTokenSource)
        {
            SalaryPaymentEntity mappedBankAccount = _mapper.Map<AccountingSalaryPaymentModel, SalaryPaymentEntity>(salaryPayment);

            int createdSalaryPaymentId = await _salaryPaymentRepository.CreateAsync(mappedBankAccount, cancellationTokenSource);

            await CreateCheckpointAsync(
                rollback: new RollbackModel()
                {
                    TransactionDate = DateTime.UtcNow,
                    TransactionIdentity = TransactionIdentity,
                    TransactionType = TransactionType.Insert,
                    RollbackItems = new List<RollbackItemModel>
                    {
                        new RollbackItemModel
                        {
                            Identity = createdSalaryPaymentId,
                            DataSet = SalaryPaymentRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            return createdSalaryPaymentId;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;
                }
            }
        }

        /// <summary>
        /// Bir işlemi geri almak için yedekleme noktası oluşturur
        /// </summary>
        /// <param name="rollback">İşlemin yedekleme noktası nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        [LogBeforeRuntimeAttr(nameof(CreateCheckpointAsync))]
        [LogAfterRuntimeAttr(nameof(CreateCheckpointAsync))]
        public async Task<int> CreateCheckpointAsync(RollbackModel rollback, CancellationTokenSource cancellationTokenSource)
        {
            RollbackEntity rollbackEntity = _mapper.Map<RollbackModel, RollbackEntity>(rollback);

            List<RollbackItemEntity> rollbackItemEntities = _mapper.Map<List<RollbackItemModel>, List<RollbackItemEntity>>(rollback.RollbackItems);

            foreach (var rollbackItemEntity in rollbackItemEntities)
            {
                rollbackItemEntity.TransactionIdentity = rollbackEntity.TransactionIdentity;

                await _transactionItemRepository.CreateAsync(rollbackItemEntity, cancellationTokenSource);
            }

            return await _transactionRepository.CreateAsync(rollbackEntity, cancellationTokenSource);
        }

        /// <summary>
        /// Bir işlemi geri alır
        /// </summary>
        /// <param name="rollback">Geri alınacak işlemin yedekleme noktası nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        [LogBeforeRuntimeAttr(nameof(RollbackTransactionAsync))]
        [LogAfterRuntimeAttr(nameof(RollbackTransactionAsync))]
        public async Task<int> RollbackTransactionAsync(RollbackModel rollback, CancellationTokenSource cancellationTokenSource)
        {
            foreach (var rollbackItem in rollback.RollbackItems)
            {
                switch (rollbackItem.DataSet?.ToString())
                {
                    case BankAccountRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _bankAccountRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _bankAccountRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _bankAccountRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else
                            throw new Exception(
                                await _translationProvider.TranslateAsync("Tanimsiz.Geri.Alma", Region, cancellationToken: cancellationTokenSource.Token));
                        break;
                    case CurrencyRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _currencyRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _currencyRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _currencyRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else
                            throw new Exception(
                                await _translationProvider.TranslateAsync("Tanimsiz.Geri.Alma", Region, cancellationToken: cancellationTokenSource.Token));
                        break;
                    case SalaryPaymentRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _salaryPaymentRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _salaryPaymentRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _salaryPaymentRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else
                            throw new Exception(
                                await _translationProvider.TranslateAsync("Tanimsiz.Geri.Alma", Region, cancellationToken: cancellationTokenSource.Token));
                        break;
                    default:
                        break;
                }
            }

            int rollbackResult = await _transactionRepository.SetRolledbackAsync(rollback.TransactionIdentity, cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            return rollbackResult;
        }

        public void DisposeInjections()
        {
            _redisCacheDataProvider.Dispose();
            _bankAccountRepository.Dispose();
            _currencyRepository.Dispose();
            _salaryPaymentRepository.Dispose();
            _transactionItemRepository.Dispose();
            _transactionRepository.Dispose();
            _translationProvider.Dispose();
            _unitOfWork.Dispose();
        }
    }
}
