using AutoMapper;

using Communication.Http.Department.Finance.Models;
using Communication.Mq.Rabbit.Publisher.Department.Buying;

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

namespace Services.Business.Departments.Finance.Services
{
    /// <summary>
    /// Karar verilen masraflar iş mantığı sınıfı
    /// </summary>
    public class CostService : BaseService, IRollbackableAsync<int>, IDisposable, IDisposableInjections
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Business.Departments.Finance.Services.CostService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Business.Departments.Finance";

        /// <summary>
        /// Önbelleğe alınan karar verilen masrafların önbellekteki adı
        /// </summary>
        private const string CACHED_DECIDED_COSTS_KEY = "Services.Business.Departments.Finance.DecidedCosts";

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
        /// Karar verilen masraflar tablosu için repository sınıfı
        /// </summary>
        private readonly DecidedCostRepository _decidedCostRepository;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

        /// <summary>
        /// Satın alınması istenilen envanterlere ait kararları rabbit kuyruğuna ekleyen nesne
        /// </summary>
        private readonly NotifyCostApprovementPublisher _notifyCostApprovementPublisher;

        /// <summary>
        /// Karar verilen masraflar iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="translationProvider">Dil çeviri sağlayıcısı sınıf</param>
        /// <param name="routeNameProvider">Servislerin rota isimlerini sağlayan sınıf</param>
        /// <param name="serviceCommunicator">Diğer servislerle iletişim kuracak ara bulucu</param>
        /// <param name="redisCacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="transactionRepository">İşlem tablosu için repository sınıfı</param>
        /// <param name="transactionItemRepository">İşlem öğesi tablosu için repository sınıfı</param>
        /// <param name="decidedCostRepository">Karar verilen masraflar tablosu için repository sınıfı</param>
        /// <param name="notifyCostApprovementPublisher">Satın alınması istenilen envanterlere ait kararları 
        /// rabbit kuyruğuna ekleyen nesne</param>
        public CostService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            TranslationProvider translationProvider,
            RedisCacheDataProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            DecidedCostRepository decidedCostRepository,
            NotifyCostApprovementPublisher notifyCostApprovementPublisher)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _redisCacheDataProvider = redisCacheDataProvider;
            _translationProvider = translationProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _decidedCostRepository = decidedCostRepository;
            _notifyCostApprovementPublisher = notifyCostApprovementPublisher;
        }

        /// <summary>
        /// Karar verilen masrafların listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<DecidedCostModel>> GetDecidedCostsAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_DECIDED_COSTS_KEY, out List<DecidedCostModel> cachedDecidedCosts)
                &&
                cachedDecidedCosts != null && cachedDecidedCosts.Any())
            {
                return cachedDecidedCosts;
            }

            List<DecidedCostEntity> decidedCosts = await _decidedCostRepository.GetListAsync(cancellationTokenSource);

            List<DecidedCostModel> mappedDecidedCosts =
                _mapper.Map<List<DecidedCostEntity>, List<DecidedCostModel>>(decidedCosts);

            _redisCacheDataProvider.Set(CACHED_DECIDED_COSTS_KEY, mappedDecidedCosts);

            return mappedDecidedCosts;
        }

        /// <summary>
        /// Yeni masraf kaydı oluşturur
        /// </summary>
        /// <param name="decidedCost">Oluşturulacak masraf kaydı nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateDecidedCostAsync(DecidedCostModel decidedCost, CancellationTokenSource cancellationTokenSource)
        {
            DecidedCostEntity mappedDecidedCost = _mapper.Map<DecidedCostModel, DecidedCostEntity>(decidedCost);

            int createdDecidedCostId = await _decidedCostRepository.CreateAsync(mappedDecidedCost, cancellationTokenSource);

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
                            Identity = createdDecidedCostId,
                            DataSet = DecidedCostRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            decidedCost.Id = createdDecidedCostId;

            if (_redisCacheDataProvider.TryGetValue(CACHED_DECIDED_COSTS_KEY, out List<DecidedCostModel> cachedIDecidedCosts)
                &&
                cachedIDecidedCosts != null)
            {
                cachedIDecidedCosts.Add(decidedCost);

                _redisCacheDataProvider.Set(CACHED_DECIDED_COSTS_KEY, cachedIDecidedCosts);
            }

            return createdDecidedCostId;
        }

        /// <summary>
        /// Bir masrafı onaylar
        /// </summary>
        /// <param name="costId">Onaylanacak masrafın Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> ApproveCostAsync(int costId, CancellationTokenSource cancellationTokenSource)
        {
            int result = await _decidedCostRepository.ApproveAsync(costId, cancellationTokenSource);

            DecidedCostEntity decidedCostEntity = await _decidedCostRepository.GetAsync(costId, cancellationTokenSource);

            if (decidedCostEntity == null)
            {
                throw new Exception("Masraf bilgisi bulunamadı");
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
                            Identity = costId,
                            DataSet = DecidedCostRepository.TABLE_NAME,
                            RollbackType = RollbackType.Update,
                            Name = nameof(DecidedCostEntity.Approved),
                            OldValue = false,
                            NewValue = true
                        },
                        new RollbackItemModel()
                        {
                            Identity = costId,
                            DataSet = DecidedCostRepository.TABLE_NAME,
                            RollbackType = RollbackType.Update,
                            Name = nameof(DecidedCostEntity.Done),
                            OldValue = false,
                            NewValue = true
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            _notifyCostApprovementPublisher.AddToBuffer(
                model: new Communication.Mq.Rabbit.Publisher.Department.Buying.Models.DecidedCostModel
                {
                    Approved = true,
                    InventoryRequestId = decidedCostEntity.InventoryRequestId,
                    Done = true
                });

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            await _notifyCostApprovementPublisher.PublishBufferAsync(cancellationTokenSource);

            _redisCacheDataProvider.RemoveObject(CACHED_DECIDED_COSTS_KEY);

            return result;
        }

        /// <summary>
        /// Bir masrafı reddeder
        /// </summary>
        /// <param name="costId">Reddedilecek masrafın Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> RejectCostAsync(int costId, CancellationTokenSource cancellationTokenSource)
        {
            int result = await _decidedCostRepository.RejectAsync(costId, cancellationTokenSource);

            DecidedCostEntity decidedCostEntity = await _decidedCostRepository.GetAsync(costId, cancellationTokenSource);

            if (decidedCostEntity == null)
            {
                throw new Exception("Masraf bilgisi bulunamadı");
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
                            Identity = costId,
                            DataSet = DecidedCostRepository.TABLE_NAME,
                            RollbackType = RollbackType.Update,
                            Name = nameof(DecidedCostEntity.Approved),
                            OldValue = false,
                            NewValue = false
                        },
                        new RollbackItemModel()
                        {
                            Identity = costId,
                            DataSet = DecidedCostRepository.TABLE_NAME,
                            RollbackType = RollbackType.Update,
                            Name = nameof(DecidedCostEntity.Done),
                            OldValue = false,
                            NewValue = true
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            _notifyCostApprovementPublisher.AddToBuffer(
                model: new Communication.Mq.Rabbit.Publisher.Department.Buying.Models.DecidedCostModel
                {
                    Approved = false,
                    InventoryRequestId = decidedCostEntity.InventoryRequestId,
                    Done = true
                });

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            await _notifyCostApprovementPublisher.PublishBufferAsync(cancellationTokenSource);

            _redisCacheDataProvider.RemoveObject(CACHED_DECIDED_COSTS_KEY);

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
                    case DecidedCostRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _decidedCostRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _decidedCostRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _decidedCostRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
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
            _decidedCostRepository.Dispose();
            _transactionItemRepository.Dispose();
            _transactionRepository.Dispose();
            _unitOfWork.Dispose();
            _translationProvider.Dispose();
        }
    }
}
