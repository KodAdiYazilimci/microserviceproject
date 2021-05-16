using AutoMapper;

using Infrastructure.Caching.Redis;
using Infrastructure.Localization.Providers;
using Infrastructure.Transaction.ExecutionHandler;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork;

using Services.Business.Departments.Accounting.Entities.Sql;
using Services.Business.Departments.Accounting.Models;
using Services.Business.Departments.Accounting.Repositories.Sql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Accounting.Services
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
        public override string ServiceName => "Services.Business.Departments.Accounting.Services.BankService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Business.Departments.Accounting";

        /// <summary>
        /// Önbelleğe alınan para birimlerinin önbellekteki adı
        /// </summary>
        private const string CACHED_CURRENCIES_KEY = "Services.Business.Departments.Accounting.Currencies";

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
        public async Task<List<BankAccountModel>> GetBankAccounts(int workerId, CancellationTokenSource cancellationTokenSource)
        {
            List<BankAccountEntity> bankAccounts =
                await _bankAccountRepository.GetBankAccountsAsync(workerId, cancellationTokenSource);

            List<BankAccountModel> mappedBankAccounts =
                _mapper.Map<List<BankAccountEntity>, List<BankAccountModel>>(bankAccounts);

            return mappedBankAccounts;
        }

        /// <summary>
        /// Yeni maaş hesabı oluşturur
        /// </summary>
        /// <param name="bankAccount">Oluşturulacak banka hesabı nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateBankAccountAsync(BankAccountModel bankAccount, CancellationTokenSource cancellationTokenSource)
        {
            BankAccountEntity mappedBankAccount = _mapper.Map<BankAccountModel, BankAccountEntity>(bankAccount);

            int createdBankAccountId = await _bankAccountRepository.CreateAsync(mappedBankAccount, cancellationTokenSource);

            await CreateCheckpointAsync(
                rollback: new RollbackModel()
                {
                    TransactionIdentity = TransactionIdentity,
                    TransactionDate = DateTime.Now,
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
        public async Task<List<CurrencyModel>> GetCurrenciesAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_CURRENCIES_KEY, out List<CurrencyModel> cureencies)
                &&
                cureencies != null && cureencies.Any())
            {
                return cureencies;
            }

            List<CurrencyEntity> currencies = await _currencyRepository.GetListAsync(cancellationTokenSource);

            List<CurrencyModel> mappedDepartments =
                _mapper.Map<List<CurrencyEntity>, List<CurrencyModel>>(currencies);

            _redisCacheDataProvider.Set(CACHED_CURRENCIES_KEY, mappedDepartments);

            return mappedDepartments;
        }

        /// <summary>
        /// Yeni para birimi oluşturur
        /// </summary>
        /// <param name="currency">Oluşturulacak para birimi nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateCurrencyAsync(CurrencyModel currency, CancellationTokenSource cancellationTokenSource)
        {
            CurrencyEntity mappedCurrency = _mapper.Map<CurrencyModel, CurrencyEntity>(currency);

            int createdCurrencyId = await _currencyRepository.CreateAsync(mappedCurrency, cancellationTokenSource);

            await CreateCheckpointAsync(
                rollback: new RollbackModel()
                {
                    TransactionDate = DateTime.Now,
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

            if (_redisCacheDataProvider.TryGetValue(CACHED_CURRENCIES_KEY, out List<CurrencyModel> cachedCurrencies) && cachedCurrencies != null)
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
        public async Task<List<SalaryPaymentModel>> GetSalaryPaymentsOfWorkerAsync(int workerId, CancellationTokenSource cancellationTokenSource)
        {
            List<SalaryPaymentEntity> bankAccounts =
           await _salaryPaymentRepository.GetSalaryPaymentsOfWorkerAsync(workerId, cancellationTokenSource);

            List<SalaryPaymentModel> mappedSalaryPayments =
                _mapper.Map<List<SalaryPaymentEntity>, List<SalaryPaymentModel>>(bankAccounts);

            return mappedSalaryPayments;
        }

        /// <summary>
        /// Yeni bir maaş ödemesi oluşturur
        /// </summary>
        /// <param name="salaryPayment">Oluşturulacak maaş ödemesinin nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateSalaryPaymentAsync(SalaryPaymentModel salaryPayment, CancellationTokenSource cancellationTokenSource)
        {
            SalaryPaymentEntity mappedBankAccount = _mapper.Map<SalaryPaymentModel, SalaryPaymentEntity>(salaryPayment);

            int createdSalaryPaymentId = await _salaryPaymentRepository.CreateAsync(mappedBankAccount, cancellationTokenSource);

            await CreateCheckpointAsync(
                rollback: new RollbackModel()
                {
                    TransactionDate = DateTime.Now,
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
