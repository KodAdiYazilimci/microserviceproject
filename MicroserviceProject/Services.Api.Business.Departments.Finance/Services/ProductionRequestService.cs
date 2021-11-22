using AutoMapper;

using Services.Communication.Http.Broker.Department.Finance.Models;
using Services.Communication.Mq.Rabbit.Publisher.Department.Selling;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Localization.Providers;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Business.Departments.Finance.Entities.Sql;
using Services.Business.Departments.Finance.Repositories.Sql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Services.Communication.Mq.Rabbit.Department.Models.Finance;

namespace Services.Business.Departments.Finance.Services
{
    /// <summary>
    /// Üretilmesi istenilen ürünler iş mantığı sınıfı
    /// </summary>
    public class ProductionRequestService : BaseService, IRollbackableAsync<int>, IDisposable, IDisposableInjections
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Business.Departments.Finance.Services.RequestService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Business.Departments.Finance";

        /// <summary>
        /// Önbelleğe alınan karar verilen üretim taleplerinin önbellekteki adı
        /// </summary>
        private const string CACHED_PRODUCTION_REQUESTS_KEY = "Services.Business.Departments.Finance.ProductionRequests";

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
        /// Üretilmesi istenilen ürünlerin talepleri tablosu için repository sınıfı
        /// </summary>
        private readonly ProductionRequestRepository _productionRequestRepository;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

        /// <summary>
        /// Üretilmesi talep edilen üretimlere ait kararları rabbit kuyruğuna ekleyen nesne
        /// </summary>
        private readonly NotifyProductionRequestApprovementPublisher _notifyProductionRequesApprovementPublisher;

        /// <summary>
        /// Üretilmesi istenilen ürünler iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="translationProvider">Dil çeviri sağlayıcısı sınıf</param>
        /// <param name="routeNameProvider">Servislerin rota isimlerini sağlayan sınıf</param>
        /// <param name="serviceCommunicator">Diğer servislerle iletişim kuracak ara bulucu</param>
        /// <param name="redisCacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="transactionRepository">İşlem tablosu için repository sınıfı</param>
        /// <param name="transactionItemRepository">İşlem öğesi tablosu için repository sınıfı</param>
        /// <param name="productionRequestRepository">Üretilmesi istenilen ürünlerin talepleri tablosu için repository sınıfı</param>
        /// <param name="notifyProductionRequesApprovementPublisher">Üretilmesi talep edilen üretimlere ait kararları rabbit kuyruğuna ekleyen nesne</param>
        public ProductionRequestService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            TranslationProvider translationProvider,
            RedisCacheDataProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            ProductionRequestRepository productionRequestRepository,
            NotifyProductionRequestApprovementPublisher notifyProductionRequesApprovementPublisher)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _redisCacheDataProvider = redisCacheDataProvider;
            _translationProvider = translationProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _productionRequestRepository = productionRequestRepository;
            _notifyProductionRequesApprovementPublisher = notifyProductionRequesApprovementPublisher;
        }

        /// <summary>
        /// Üretim taleplerinin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<ProductionRequestModel>> GetProductionRequestsAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_PRODUCTION_REQUESTS_KEY, out List<ProductionRequestModel> cachedProductionRequests)
                &&
                cachedProductionRequests != null && cachedProductionRequests.Any())
            {
                return cachedProductionRequests;
            }

            List<ProductionRequestEntity> productionRequests = await _productionRequestRepository.GetListAsync(cancellationTokenSource);

            List<ProductionRequestModel> mappedProductionRequests =
                _mapper.Map<List<ProductionRequestEntity>, List<ProductionRequestModel>>(productionRequests);

            _redisCacheDataProvider.Set(CACHED_PRODUCTION_REQUESTS_KEY, mappedProductionRequests);

            return mappedProductionRequests;
        }

        /// <summary>
        /// Ürün üretim talebi oluşturur
        /// </summary>
        /// <param name="productionRequest">Talebi oluşturulacak üretim modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateProductionRequestAsync(ProductionRequestModel productionRequest, CancellationTokenSource cancellationTokenSource)
        {
            ProductionRequestEntity mappedProductionRequest = _mapper.Map<ProductionRequestModel, ProductionRequestEntity>(productionRequest);

            int createdProductionRequestId = await _productionRequestRepository.CreateAsync(mappedProductionRequest, cancellationTokenSource);

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
                            Identity = createdProductionRequestId,
                            DataSet = ProductionRequestRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            mappedProductionRequest.Id = createdProductionRequestId;

            if (_redisCacheDataProvider.TryGetValue(CACHED_PRODUCTION_REQUESTS_KEY, out List<ProductionRequestModel> cachedProductionRequests)
                &&
                cachedProductionRequests != null)
            {
                cachedProductionRequests.Add(productionRequest);

                _redisCacheDataProvider.Set(CACHED_PRODUCTION_REQUESTS_KEY, cachedProductionRequests);
            }

            return createdProductionRequestId;
        }

        /// <summary>
        /// Bir üretim talebini onaylar
        /// </summary>
        /// <param name="productionRequestId">Onaylanacak üretim talebinin Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> ApproveProductionRequestAsync(int productionRequestId, CancellationTokenSource cancellationTokenSource)
        {
            int result = await _productionRequestRepository.ApproveAsync(productionRequestId, cancellationTokenSource);

            ProductionRequestEntity productionRequestEntity = await _productionRequestRepository.GetAsync(productionRequestId, cancellationTokenSource);

            if (productionRequestEntity == null)
            {
                throw new Exception("Üretim talebi bilgisi bulunamadı");
            }

            await CreateCheckpointAsync(
                rollback: new RollbackModel()
                {
                    TransactionType = TransactionType.Update,
                    TransactionDate = DateTime.Now,
                    TransactionIdentity = TransactionIdentity,
                    RollbackItems = new List<RollbackItemModel>
                    {
                        new RollbackItemModel()
                        {
                            Identity = productionRequestId,
                            DataSet = ProductionRequestRepository.TABLE_NAME,
                            RollbackType = RollbackType.Update,
                            Name = nameof(ProductionRequestEntity.Approved),
                            OldValue = false,
                            NewValue = true
                        },
                        new RollbackItemModel()
                        {
                            Identity = productionRequestId,
                            DataSet = ProductionRequestRepository.TABLE_NAME,
                            RollbackType = RollbackType.Update,
                            Name = nameof(ProductionRequestEntity.Done),
                            OldValue = false,
                            NewValue = true
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            _notifyProductionRequesApprovementPublisher.AddToBuffer(
                model: new Communication.Mq.Rabbit.Department.Models.Selling.ProductionRequestQueueModel
                {
                    Approved = true,
                    Amount = productionRequestEntity.Amount,
                    DepartmentId = productionRequestEntity.DepartmentId,
                    ProductId = productionRequestEntity.ProductId,
                    ReferenceNumber = productionRequestEntity.ReferenceNumber
                });

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            await _notifyProductionRequesApprovementPublisher.PublishBufferAsync(cancellationTokenSource);

            _redisCacheDataProvider.RemoveObject(CACHED_PRODUCTION_REQUESTS_KEY);

            return result;
        }

        /// <summary>
        /// Bir üretim talebini reddeder
        /// </summary>
        /// <param name="productionRequestId">Reddedilecek üretim talebinin Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> RejectProductionRequestAsync(int productionRequestId, CancellationTokenSource cancellationTokenSource)
        {
            int result = await _productionRequestRepository.RejectAsync(productionRequestId, cancellationTokenSource);

            ProductionRequestEntity productionRequestEntity = await _productionRequestRepository.GetAsync(productionRequestId, cancellationTokenSource);

            if (productionRequestEntity == null)
            {
                throw new Exception("Üretim talebi bilgisi bulunamadı");
            }

            await CreateCheckpointAsync(
                rollback: new RollbackModel()
                {
                    TransactionType = TransactionType.Update,
                    TransactionDate = DateTime.Now,
                    TransactionIdentity = TransactionIdentity,
                    RollbackItems = new List<RollbackItemModel>
                    {
                        new RollbackItemModel()
                        {
                            Identity = productionRequestId,
                            DataSet = ProductionRequestRepository.TABLE_NAME,
                            RollbackType = RollbackType.Update,
                            Name = nameof(ProductionRequestEntity.Approved),
                            OldValue = false,
                            NewValue = false
                        },
                        new RollbackItemModel()
                        {
                            Identity = productionRequestId,
                            DataSet = ProductionRequestRepository.TABLE_NAME,
                            RollbackType = RollbackType.Update,
                            Name = nameof(ProductionRequestEntity.Done),
                            OldValue = false,
                            NewValue = true
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            _notifyProductionRequesApprovementPublisher.AddToBuffer(
                model: new Communication.Mq.Rabbit.Department.Models.Selling.ProductionRequestQueueModel
                {
                    Approved = false,
                    Amount = productionRequestEntity.Amount,
                    ReferenceNumber = productionRequestEntity.ReferenceNumber,
                    ProductId = productionRequestEntity.ProductId,
                    DepartmentId = productionRequestEntity.DepartmentId
                });

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            await _notifyProductionRequesApprovementPublisher.PublishBufferAsync(cancellationTokenSource);

            _redisCacheDataProvider.RemoveObject(CACHED_PRODUCTION_REQUESTS_KEY);

            return result;
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
                    case ProductionRequestRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _productionRequestRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _productionRequestRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _productionRequestRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
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
            _productionRequestRepository.Dispose();
            _transactionItemRepository.Dispose();
            _transactionRepository.Dispose();
            _unitOfWork.Dispose();
            _translationProvider.Dispose();
        }
    }
}
