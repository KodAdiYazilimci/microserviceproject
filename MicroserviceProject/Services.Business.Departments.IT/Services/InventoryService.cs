using AutoMapper;

using Services.Communication.Http.Broker.Department.IT.Models;
using Services.Communication.Mq.Rabbit.Publisher.Department.Buying;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Localization.Providers;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Business.Departments.IT.Entities.Sql;
using Services.Business.Departments.IT.Repositories.Sql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.IT.Services
{
    /// <summary>
    /// Envanter işlemleri iş mantığı sınıfı
    /// </summary>
    public class InventoryService : BaseService, IRollbackableAsync<int>, IDisposable, IDisposableInjections
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Business.Departments.IT.Services.InventoryService";

        /// <summary>
        /// Önbelleğe alınan envanterlerin önbellekteki adı
        /// </summary>
        private const string CACHED_INVENTORIES_KEY = "Services.Business.Departments.IT.Inventories";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Business.Departments.IT";

        /// <summary>
        /// Önbelleğe alınan varsayılan envanterlerin önbellekteki adı
        /// </summary>
        private const string CACHED_INVENTORIES_DEFAULTS_KEY = "Services.Business.Departments.IT.InventoryDefaults";

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
        /// Envanter tablosu için repository sınıfı
        /// </summary>
        private readonly InventoryRepository _inventoryRepository;

        /// <summary>
        /// Varsayılan envanterler tablosu için repository sınıfı
        /// </summary>
        private readonly InventoryDefaultsRepository _inventoryDefaultsRepository;

        /// <summary>
        /// Çalışan envanterleri tablosu için repository sınıfı
        /// </summary>
        private readonly WorkerInventoryRepository _workerInventoryRepository;

        /// <summary>
        /// Çalışanlara verilecek stoğu olmayan envanterler tablosu nesnesi
        /// </summary>
        private readonly PendingWorkerInventoryRepository _pendingWorkerInventoryRepository;

        /// <summary>
        /// Satınalma departmanına tükenen envanter için alım talebi kuyruğuna kayıt ekleyecek nesne
        /// </summary>
        private readonly CreateInventoryRequestPublisher _createInventoryRequestPublisher;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

        /// <summary>
        /// Envanter işlemleri iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="translationProvider">Dil çeviri sağlayıcısı sınıf</param>
        /// <param name="redisCacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="createInventoryRequestPublisher">Satınalma departmanına tükenen envanter için alım talebi kuyruğuna kayıt ekleyecek nesne</param>
        /// <param name="inventoryRepository">Envanter tablosu için repository sınıfı</param>
        /// <param name="inventoryDefaultsRepository">Varsayılan envanterler tablosu için repository sınıfı</param>
        /// <param name="workerInventoryRepository">Çalışan envanterleri tablosu için repository sınıfı</param>
        /// <param name="pendingWorkerInventoryRepository">Çalışanlara verilecek stoğu olmayan envanterler tablosu nesnesi</param>
        public InventoryService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            TranslationProvider translationProvider,
            RedisCacheDataProvider redisCacheDataProvider,
            CreateInventoryRequestPublisher createInventoryRequestPublisher,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            InventoryRepository inventoryRepository,
            InventoryDefaultsRepository inventoryDefaultsRepository,
            WorkerInventoryRepository workerInventoryRepository,
            PendingWorkerInventoryRepository pendingWorkerInventoryRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _redisCacheDataProvider = redisCacheDataProvider;
            _translationProvider = translationProvider;

            _createInventoryRequestPublisher = createInventoryRequestPublisher;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _inventoryRepository = inventoryRepository;
            _inventoryDefaultsRepository = inventoryDefaultsRepository;
            _workerInventoryRepository = workerInventoryRepository;
            _pendingWorkerInventoryRepository = pendingWorkerInventoryRepository;
        }

        /// <summary>
        /// Satın alınmayı bekleyen envanterle ilgili çalışana envanter ataması yapar veya alımı erteler
        /// </summary>
        /// <param name="inventoryRequest">Envanter talebi nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task InformInventoryRequestAsync(InventoryRequestModel inventoryRequest, CancellationTokenSource cancellationTokenSource)
        {
            if (inventoryRequest.Revoked)
                await _inventoryRepository.IncreaseStockCountAsync(inventoryRequest.InventoryId, inventoryRequest.Amount, cancellationTokenSource);

            List<PendingWorkerInventoryEntity> pendingInventories = await _pendingWorkerInventoryRepository.GetListAsync(cancellationTokenSource);

            foreach (var pendingWorkerInventory in pendingInventories.Where(x => x.InventoryId == inventoryRequest.InventoryId).ToList())
            {
                if (inventoryRequest.Revoked && inventoryRequest.Amount > 0)
                {
                    await _workerInventoryRepository.CreateAsync(
                        workerInventory: new WorkerInventoryEntity
                        {
                            FromDate = pendingWorkerInventory.FromDate,
                            ToDate = pendingWorkerInventory.ToDate,
                            WorkerId = pendingWorkerInventory.WorkerId,
                            InventoryId = pendingWorkerInventory.InventoryId
                        },
                        cancellationTokenSource: cancellationTokenSource);

                    await _inventoryRepository.DescendStockCountAsync(inventoryRequest.InventoryId, 1, cancellationTokenSource);

                    await _pendingWorkerInventoryRepository.SetCompleteAsync(pendingWorkerInventory.WorkerId, pendingWorkerInventory.InventoryId, cancellationTokenSource);

                    inventoryRequest.Amount -= 1;
                }
                else if (!inventoryRequest.Revoked)
                {
                    await _pendingWorkerInventoryRepository.DelayAsync(pendingWorkerInventory.WorkerId, pendingWorkerInventory.InventoryId, DateTime.Now.AddDays(7), cancellationTokenSource);
                }
            }

            await _unitOfWork.SaveAsync(cancellationTokenSource);
        }

        /// <summary>
        /// Envanterlerin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<InventoryModel>> GetInventoriesAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_INVENTORIES_KEY, out List<InventoryModel> cachedInventories)
                &&
                cachedInventories != null && cachedInventories.Any())
            {
                return cachedInventories;
            }

            List<InventoryEntity> inventories = await _inventoryRepository.GetListAsync(cancellationTokenSource);

            List<InventoryModel> mappedInventories =
                _mapper.Map<List<InventoryEntity>, List<InventoryModel>>(inventories);

            _redisCacheDataProvider.Set(CACHED_INVENTORIES_KEY, mappedInventories);

            return mappedInventories;
        }

        /// <summary>
        /// Yeni envanter oluşturur
        /// </summary>
        /// <param name="inventory">Oluşturulacak envanter nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateInventoryAsync(InventoryModel inventory, CancellationTokenSource cancellationTokenSource)
        {
            InventoryEntity mappedInventory = _mapper.Map<InventoryModel, InventoryEntity>(inventory);

            int createdInventoryId = await _inventoryRepository.CreateAsync(mappedInventory, cancellationTokenSource);

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
                            Identity = createdInventoryId,
                            DataSet = InventoryRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            inventory.Id = createdInventoryId;

            if (_redisCacheDataProvider.TryGetValue(CACHED_INVENTORIES_KEY, out List<InventoryModel> cachedInventories) && cachedInventories != null)
            {
                cachedInventories.Add(inventory);

                _redisCacheDataProvider.Set(CACHED_INVENTORIES_KEY, cachedInventories);
            }

            return createdInventoryId;
        }

        /// <summary>
        /// Yeni çalışan için varsayılan envanter ataması yapar
        /// </summary>
        /// <param name="inventory">Atanacak envanter</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<InventoryModel> CreateDefaultInventoryForNewWorkerAsync(InventoryModel inventory, CancellationTokenSource cancellationTokenSource)
        {
            List<InventoryModel> existingInventories = await GetInventoriesAsync(cancellationTokenSource);

            if (!existingInventories.Any(x => x.Id == inventory.Id))
            {
                throw new Exception("Id ye ait envanter bulunamadı");
            }

            if (await _inventoryDefaultsRepository.CheckExistAsync(inventory.Id, cancellationTokenSource))
            {
                throw new Exception("Bu envanter zaten atanmış");
            }

            int createdInventoryDefaultId = await _inventoryDefaultsRepository.CreateAsync(
                 inventoryDefault: new InventoryDefaultsEntity()
                 {
                     InventoryId = inventory.Id,
                     ForNewWorker = true
                 },
                 cancellationTokenSource: cancellationTokenSource);

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
                            Identity = createdInventoryDefaultId,
                            DataSet = InventoryDefaultsRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            if (_redisCacheDataProvider.TryGetValue(CACHED_INVENTORIES_DEFAULTS_KEY, out List<InventoryModel> cachedInventories)
                  &&
                  cachedInventories != null && cachedInventories.Any())
            {
                cachedInventories.Add(existingInventories.FirstOrDefault(x => x.Id == inventory.Id));

                _redisCacheDataProvider.Set(CACHED_INVENTORIES_DEFAULTS_KEY, cachedInventories);
            }

            return inventory;
        }

        /// <summary>
        /// Yeni çalışanlara verilecek envanterlerin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public List<InventoryModel> GetInventoriesForNewWorker(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_INVENTORIES_DEFAULTS_KEY, out List<InventoryModel> cachedInventories)
                &&
                cachedInventories != null && cachedInventories.Any())
            {
                return cachedInventories;
            }

            Task<List<InventoryEntity>> inventoriesTask = _inventoryRepository.GetListAsync(cancellationTokenSource);
            Task<List<InventoryDefaultsEntity>> inventoryDefaultsTask = _inventoryDefaultsRepository.GetListAsync(cancellationTokenSource);

            Task.WaitAll(new Task[] { inventoriesTask, inventoryDefaultsTask });

            List<InventoryModel> inventories = (from inv in inventoriesTask.Result
                                                join def in inventoryDefaultsTask.Result
                                                on
                                                inv.Id equals def.InventoryId
                                                where
                                                def.ForNewWorker
                                                select
                                                new InventoryModel()
                                                {
                                                    Id = inv.Id,
                                                    Name = inv.Name
                                                }).ToList();

            _redisCacheDataProvider.Set(CACHED_INVENTORIES_DEFAULTS_KEY, inventories, DateTime.Now.AddMinutes(10));

            return inventories;
        }

        /// <summary>
        /// Bir çalışana envanter ataması yapar
        /// </summary>
        /// <param name="worker">Envanter bilgisini içeren çalışan nesnesi</param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public async Task<WorkerModel> AssignInventoryToWorkerAsync(WorkerModel worker, CancellationTokenSource cancellationTokenSource)
        {
            List<int> inventoryIds = worker.ITInventories.Select(x => x.Id).ToList();

            List<InventoryEntity> inventories =
                await _inventoryRepository.GetForSpecificIdAsync(inventoryIds, cancellationTokenSource);

            foreach (var inventoryId in inventoryIds)
            {
                if (!inventories.Select(x => x.Id).Contains(inventoryId))
                {
                    throw new Exception($"{inventoryId} Id değerine sahip envanter bulunamadı");
                }

                InventoryEntity inventoryEntity = inventories.FirstOrDefault(x => x.Id == inventoryId);

                if (inventoryEntity.CurrentStockCount <= 0)
                {
                    _createInventoryRequestPublisher.AddToBuffer(new  Communication.Mq.Rabbit.Publisher.Department.Buying.Models.InventoryRequestModel()
                    {
                        Amount = 3,
                        DepartmentId = (int)Constants.Departments.InformationTechnologies,
                        InventoryId = inventoryId
                    });

                    worker.ITInventories.FirstOrDefault(x => x.Id == inventoryId).CurrentStockCount = 0;
                }
                else
                {
                    await _inventoryRepository.DescendStockCountAsync(inventoryId, 1, cancellationTokenSource);

                    await CreateCheckpointAsync(
                        rollback: new RollbackModel()
                        {
                            TransactionIdentity = TransactionIdentity,
                            TransactionDate = DateTime.Now,
                            TransactionType = TransactionType.Update,
                            RollbackItems = new List<RollbackItemModel>
                            {
                                new RollbackItemModel
                                {
                                    Identity = inventoryId,
                                    DataSet = InventoryRepository.TABLE_NAME,
                                    RollbackType = RollbackType.IncreaseValue,
                                    Difference = 1
                                }
                            }
                        },
                        cancellationTokenSource: cancellationTokenSource);

                    _redisCacheDataProvider.RemoveObject(CACHED_INVENTORIES_KEY);
                }
            }

            foreach (var inventoryModel in worker.ITInventories)
            {
                if (inventoryModel.CurrentStockCount > 0)
                {
                    // TODO: Transaction tablosuna kayıt eklenecek

                    int createdInventoryId = await _workerInventoryRepository.CreateAsync(new WorkerInventoryEntity
                    {
                        FromDate = worker.FromDate,
                        ToDate = worker.ToDate,
                        InventoryId = inventoryModel.Id,
                        WorkerId = worker.Id
                    }, cancellationTokenSource);

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
                                    Identity = createdInventoryId,
                                    DataSet = WorkerInventoryRepository.TABLE_NAME,
                                    RollbackType = RollbackType.Delete
                                }
                            }
                        },
                        cancellationTokenSource: cancellationTokenSource);
                }
                else
                {
                    // TODO: Transaction tablosuna kayıt eklenecek

                    int createdPendingInventoryId = await _pendingWorkerInventoryRepository.CreateAsync(new PendingWorkerInventoryEntity()
                    {
                        FromDate = worker.FromDate,
                        InventoryId = inventoryModel.Id,
                        StockCount = 1,
                        ToDate = worker.ToDate,
                        WorkerId = worker.Id
                    }, cancellationTokenSource);

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
                                    Identity = createdPendingInventoryId,
                                    DataSet = PendingWorkerInventoryRepository.TABLE_NAME,
                                    RollbackType  = RollbackType.Delete
                                }
                            }
                        },
                        cancellationTokenSource: cancellationTokenSource);
                }
            }

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            await _createInventoryRequestPublisher.PublishBufferAsync(cancellationTokenSource);

            return worker;
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
                    case InventoryRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _inventoryRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _inventoryRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _inventoryRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.IncreaseValue)
                        {
                            await _inventoryRepository.IncreaseStockCountAsync((int)rollbackItem.Identity, (int)rollbackItem.Difference, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.DecreaseValue)
                        {
                            await _inventoryRepository.DescendStockCountAsync((int)rollbackItem.Identity, (int)rollbackItem.Difference, cancellationTokenSource);
                        }
                        else
                            throw new Exception(
                                await _translationProvider.TranslateAsync("Tanimsiz.Geri.Alma", Region, cancellationToken: cancellationTokenSource.Token));
                        break;
                    case InventoryDefaultsRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _inventoryDefaultsRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _inventoryDefaultsRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _inventoryDefaultsRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else
                           throw new Exception(
                                await _translationProvider.TranslateAsync("Tanimsiz.Geri.Alma", Region, cancellationToken: cancellationTokenSource.Token));
                        break;
                    case WorkerInventoryRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _workerInventoryRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _inventoryDefaultsRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _inventoryDefaultsRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
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
            _inventoryRepository.Dispose();
            _inventoryDefaultsRepository.Dispose();
            _pendingWorkerInventoryRepository.Dispose();
            _transactionItemRepository.Dispose();
            _transactionRepository.Dispose();
            _workerInventoryRepository.Dispose();
            _unitOfWork.Dispose();
            _translationProvider.Dispose();
        }
    }
}
