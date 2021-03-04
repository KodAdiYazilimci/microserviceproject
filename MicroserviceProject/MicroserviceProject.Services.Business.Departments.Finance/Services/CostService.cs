using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator;
using MicroserviceProject.Infrastructure.Routing.Providers;
using MicroserviceProject.Services.Business.Departments.Finance.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.Finance.Repositories.Sql;
using MicroserviceProject.Services.Model.Department.Buying;
using MicroserviceProject.Services.Model.Department.Finance;
using MicroserviceProject.Services.Transaction;
using MicroserviceProject.Services.Transaction.Models;
using MicroserviceProject.Services.Transaction.Types;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Finance.Services
{
    /// <summary>
    /// Karar verilen masraflar iş mantığı sınıfı
    /// </summary>
    public class CostService : BaseService, IRollbackableAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "MicroserviceProject.Services.Business.Departments.Finance.Services.CostService";

        /// <summary>
        /// Önbelleğe alınan karar verilen masrafların önbellekteki adı
        /// </summary>
        private const string CACHED_DECIDED_COSTS_KEY = "MicroserviceProject.Services.Business.Departments.Buying.InventoryRequests";

        /// <summary>
        /// Rediste tutulan önbellek yönetimini sağlayan sınıf
        /// </summary>
        private readonly CacheDataProvider _cacheDataProvider;

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
        /// Servislerin rota isimlerini sağlayan sınıf
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Diğer servislerle iletişim kuracak ara bulucu
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Karar verilen masraflar iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="routeNameProvider">Servislerin rota isimlerini sağlayan sınıf</param>
        /// <param name="serviceCommunicator">Diğer servislerle iletişim kuracak ara bulucu</param>
        /// <param name="cacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="transactionRepository">İşlem tablosu için repository sınıfı</param>
        /// <param name="transactionItemRepository">İşlem öğesi tablosu için repository sınıfı</param>
        /// <param name="decidedCostRepository">Karar verilen masraflar tablosu için repository sınıfı</param>
        public CostService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator,
            CacheDataProvider cacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            DecidedCostRepository decidedCostRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cacheDataProvider = cacheDataProvider;
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _decidedCostRepository = decidedCostRepository;
        }

        /// <summary>
        /// Karar verilen masrafların listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<DecidedCostModel>> GetDecidedCostsAsync(CancellationToken cancellationToken)
        {
            if (_cacheDataProvider.TryGetValue(CACHED_DECIDED_COSTS_KEY, out List<DecidedCostModel> cachedDecidedCosts)
                &&
                cachedDecidedCosts != null && cachedDecidedCosts.Any())
            {
                return cachedDecidedCosts;
            }

            List<DecidedCostEntity> decidedCosts = await _decidedCostRepository.GetListAsync(cancellationToken);

            List<DecidedCostModel> mappedDecidedCosts =
                _mapper.Map<List<DecidedCostEntity>, List<DecidedCostModel>>(decidedCosts);

            _cacheDataProvider.Set(CACHED_DECIDED_COSTS_KEY, mappedDecidedCosts);

            return mappedDecidedCosts;
        }

        /// <summary>
        /// Yeni masraf kaydı oluşturur
        /// </summary>
        /// <param name="decidedCost">Oluşturulacak masraf kaydı nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateDecidedCostAsync(DecidedCostModel decidedCost, CancellationToken cancellationToken)
        {
            DecidedCostEntity mappedDecidedCost = _mapper.Map<DecidedCostModel, DecidedCostEntity>(decidedCost);

            int createdDecidedCostId = await _decidedCostRepository.CreateAsync(mappedDecidedCost, cancellationToken);

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
                cancellationToken: cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            decidedCost.Id = createdDecidedCostId;

            if (_cacheDataProvider.TryGetValue(CACHED_DECIDED_COSTS_KEY, out List<DecidedCostModel> cachedIDecidedCosts)
                &&
                cachedIDecidedCosts != null)
            {
                cachedIDecidedCosts.Add(decidedCost);

                _cacheDataProvider.Set(CACHED_DECIDED_COSTS_KEY, cachedIDecidedCosts);
            }

            return createdDecidedCostId;
        }

        /// <summary>
        /// Bir masrafı onaylar
        /// </summary>
        /// <param name="costId">Onaylanacak masrafın Id değeri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> ApproveCostAsync(int costId, CancellationToken cancellationToken)
        {
            int result = await _decidedCostRepository.ApproveAsync(costId, cancellationToken);

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
                cancellationToken: cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            _cacheDataProvider.RemoveObject(CACHED_DECIDED_COSTS_KEY);

            return result;
        }

        /// <summary>
        /// Bir masrafı reddeder
        /// </summary>
        /// <param name="costId">Reddedilecek masrafın Id değeri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> RejectCostAsync(int costId, CancellationToken cancellationToken)
        {
            int result = await _decidedCostRepository.RejectAsync(costId, cancellationToken);

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
                cancellationToken: cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            _cacheDataProvider.RemoveObject(CACHED_DECIDED_COSTS_KEY);

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
                    _cacheDataProvider.Dispose();
                    _decidedCostRepository.Dispose();
                    _transactionItemRepository.Dispose();
                    _transactionRepository.Dispose();
                    _unitOfWork.Dispose();
                }

                disposed = true;

                Dispose();
            }
        }

        /// <summary>
        /// Bir işlemi geri almak için yedekleme noktası oluşturur
        /// </summary>
        /// <param name="rollback">İşlemin yedekleme noktası nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        public async Task<int> CreateCheckpointAsync(RollbackModel rollback, CancellationToken cancellationToken)
        {
            RollbackEntity rollbackEntity = _mapper.Map<RollbackModel, RollbackEntity>(rollback);

            List<RollbackItemEntity> rollbackItemEntities = _mapper.Map<List<RollbackItemModel>, List<RollbackItemEntity>>(rollback.RollbackItems);

            foreach (var rollbackItemEntity in rollbackItemEntities)
            {
                rollbackItemEntity.TransactionIdentity = rollbackEntity.TransactionIdentity;

                await _transactionItemRepository.CreateAsync(rollbackItemEntity, cancellationToken);
            }

            return await _transactionRepository.CreateAsync(rollbackEntity, cancellationToken);
        }

        /// <summary>
        /// Bir işlemi geri alır
        /// </summary>
        /// <param name="rollback">Geri alınacak işlemin yedekleme noktası nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        public async Task<int> RollbackTransactionAsync(RollbackModel rollback, CancellationToken cancellationToken)
        {
            foreach (var rollbackItem in rollback.RollbackItems)
            {
                switch (rollbackItem.DataSet?.ToString())
                {
                    case DecidedCostRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _decidedCostRepository.DeleteAsync((int)rollbackItem.Identity, cancellationToken);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _decidedCostRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationToken);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _decidedCostRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationToken);
                        }
                        else
                            throw new Exception("Tanımlanmamış geri alma biçimi");
                        break;
                    default:
                        break;
                }
            }

            int rollbackResult = await _transactionRepository.SetRolledbackAsync(rollback.TransactionIdentity, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return rollbackResult;
        }
    }
}
