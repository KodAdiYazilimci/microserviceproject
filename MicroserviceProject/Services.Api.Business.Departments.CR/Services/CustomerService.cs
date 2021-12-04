using AutoMapper;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Communication.Http.Wrapper.Disposing;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.EntityFramework;

using Services.Api.Business.Departments.CR.Configuration.Persistence;
using Services.Api.Business.Departments.CR.Entities.EntityFramework;
using Services.Api.Business.Departments.CR.Repositories.EntityFramework;
using Services.Communication.Http.Broker.Department.CR.Models;
using Services.Communication.Http.Broker.Localization;
using Services.Communication.Http.Broker.Localization.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.CR.Services
{
    /// <summary>
    /// Müşteriler iş mantığı sınıfı
    /// </summary>
    public class CustomerService : BaseService, IRollbackableAsync, IAsyncDisposable, IDisposableInjectionsAsync
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Api.Business.Departments.CR.Services.CustomerService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Api.Business.Departments.CR";

        /// <summary>
        /// Önbelleğe alınan müşterilerin önbellekteki adı
        /// </summary>
        private const string CACHED_CUSTOMERS_KEY = "Services.Api.Business.Departments.CR.Customers";

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
        /// Müşteriler repository sınıfı
        /// </summary>
        private readonly CustomerRepository _customerRepository;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork<CRContext> _unitOfWork;

        private readonly LocalizationCommunicator _localizationCommunicator;

        /// <summary>
        /// Müşteriler iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="translationProvider">Dil çeviri sağlayıcısı sınıf nesnesi</param>
        /// <param name="redisCacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf nesnesi</param>
        /// <param name="transactionRepository">İşlem tablosu için repository sınıfı nesnesi</param>
        /// <param name="transactionItemRepository">İşlem öğesi tablosu için repository sınıfı nesnesi</param>
        /// <param name="customerRepository">Müşteriler repository sınıfı nesnesi</param>
        public CustomerService(
            IMapper mapper,
            IUnitOfWork<CRContext> unitOfWork,
            RedisCacheDataProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            CustomerRepository customerRepository, 
            LocalizationCommunicator localizationCommunicator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _redisCacheDataProvider = redisCacheDataProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;
            _customerRepository = customerRepository;
            _localizationCommunicator = localizationCommunicator;
        }

        /// <summary>
        /// Müşterilerin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<CustomerModel>> GetCustomersAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_CUSTOMERS_KEY, out List<CustomerModel> cachedCustomers)
                &&
                cachedCustomers != null && cachedCustomers.Any())
            {
                return cachedCustomers;
            }

            List<CustomerEntity> customers = await _customerRepository.GetListAsync(cancellationTokenSource);

            List<CustomerModel> mappedCustomers = _mapper.Map<List<CustomerEntity>, List<CustomerModel>>(customers);

            _redisCacheDataProvider.Set(CACHED_CUSTOMERS_KEY, mappedCustomers);

            return mappedCustomers;
        }

        /// <summary>
        /// Yeni bir müşteri oluşturur
        /// </summary>
        /// <param name="customerModel">Oluşturulacak müşteri modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateCustomerAsync(CustomerModel customerModel, CancellationTokenSource cancellationTokenSource)
        {
            CustomerEntity mappedCustomerEntity = _mapper.Map<CustomerModel, CustomerEntity>(customerModel);

            await _customerRepository.CreateAsync(mappedCustomerEntity, cancellationTokenSource);

            await CreateCheckpointAsync(
                rollback: new RollbackModel()
                {
                    TransactionType = TransactionType.Insert,
                    TransactionDate = DateTime.Now,
                    TransactionIdentity = TransactionIdentity,
                    RollbackItems = new List<RollbackItemModel>
                    {
                        new RollbackItemModel()
                        {
                            Identity = mappedCustomerEntity.Id,
                            DataSet = CustomerRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            if (_redisCacheDataProvider.TryGetValue(CACHED_CUSTOMERS_KEY, out List<CustomerModel> cachedCustomers)
                &&
                cachedCustomers != null)
            {
                customerModel.Id = mappedCustomerEntity.Id;

                cachedCustomers.Add(customerModel);

                _redisCacheDataProvider.Set(CACHED_CUSTOMERS_KEY, cachedCustomers);
            }

            return mappedCustomerEntity.Id;
        }

        /// <summary>
        /// Bir işlemi geri almak için yedekleme noktası oluşturur
        /// </summary>
        /// <param name="rollback">İşlemin yedekleme noktası nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        public async Task CreateCheckpointAsync(RollbackModel rollback, CancellationTokenSource cancellationTokenSource)
        {
            RollbackEntity rollbackEntity = _mapper.Map<RollbackModel, RollbackEntity>(rollback);

            List<RollbackItemEntity> rollbackItemEntities = _mapper.Map<List<RollbackItemModel>, List<RollbackItemEntity>>(rollback.RollbackItems);

            foreach (var rollbackItemEntity in rollbackItemEntities)
            {
                rollbackItemEntity.TransactionIdentity = rollbackEntity.TransactionIdentity;

                await _transactionItemRepository.CreateAsync(rollbackItemEntity, cancellationTokenSource);
            }

            await _transactionRepository.CreateAsync(rollbackEntity, cancellationTokenSource);
        }

        /// <summary>
        /// Bir işlemi geri alır
        /// </summary>
        /// <param name="rollback">Geri alınacak işlemin yedekleme noktası nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        public async Task RollbackTransactionAsync(RollbackModel rollback, CancellationTokenSource cancellationTokenSource)
        {
            foreach (var rollbackItem in rollback.RollbackItems)
            {
                switch (rollbackItem.DataSet?.ToString())
                {
                    case CustomerRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _customerRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _customerRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _customerRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else
                        {
                            ServiceResultModel<TranslationModel> translationServiceResult =
                                await _localizationCommunicator.TranslateAsync("Tanimsiz.Geri.Alma", Region, null, cancellationTokenSource: cancellationTokenSource);

                            if (translationServiceResult.IsSuccess)
                            {
                                throw new Exception(translationServiceResult.Data.Text);
                            }
                            else
                            {
                                throw new Exception(translationServiceResult.ErrorModel.Description);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            await _transactionRepository.SetRolledbackAsync(rollback.TransactionIdentity, cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);
        }

        public async Task DisposeInjectionsAsync()
        {
            _redisCacheDataProvider.Dispose();
            await _customerRepository.DisposeAsync();
            await _transactionItemRepository.DisposeAsync();
            await _transactionRepository.DisposeAsync();
            await _unitOfWork.DisposeAsync();
            _localizationCommunicator.Dispose();
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <returns></returns>
        public ValueTask DisposeAsync()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

            return ValueTask.CompletedTask;
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
    }
}
