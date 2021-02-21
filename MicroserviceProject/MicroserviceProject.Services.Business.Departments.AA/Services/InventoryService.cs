using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Services.Business.Departments.AA.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.AA.Repositories.Sql;
using MicroserviceProject.Services.Model.Department.AA;
using MicroserviceProject.Services.Model.Department.HR;
using MicroserviceProject.Services.Transaction;
using MicroserviceProject.Services.Transaction.Models;
using MicroserviceProject.Services.Transaction.Types;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.AA.Services
{
    /// <summary>
    /// Envanter işlemleri iş mantığı sınıfı
    /// </summary>
    public class InventoryService : IRollbackableAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Önbelleğe alınan envanterlerin önbellekteki adı
        /// </summary>
        private const string CACHED_INVENTORIES_KEY = "MicroserviceProject.Services.Business.Departments.AA.Inventories";

        /// <summary>
        /// Önbelleğe alınan varsayılan envanterlerin önbellekteki adı
        /// </summary>
        private const string CACHED_INVENTORIES_DEFAULTS_KEY = "MicroserviceProject.Services.Business.Departments.AA.InventoryDefaults";

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
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Envanter işlemleri iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="cacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="transactionRepository">İşlem tablosu için repository sınıfı</param>
        /// <param name="transactionItemRepository">İşlem öğesi tablosu için repository sınıfı</param>
        /// <param name="inventoryRepository">Envanter tablosu için repository sınıfı</param>
        /// <param name="inventoryDefaultsRepository">Varsayılan envanterler tablosu için repository sınıfı</param>
        /// <param name="workerInventoryRepository">Çalışan envanterleri tablosu için repository sınıfı</param>
        public InventoryService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            CacheDataProvider cacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            InventoryRepository inventoryRepository,
            InventoryDefaultsRepository inventoryDefaultsRepository,
            WorkerInventoryRepository workerInventoryRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cacheDataProvider = cacheDataProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _inventoryRepository = inventoryRepository;
            _inventoryDefaultsRepository = inventoryDefaultsRepository;
            _workerInventoryRepository = workerInventoryRepository;
        }

        /// <summary>
        /// Envanterlerin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<InventoryModel>> GetInventoriesAsync(CancellationToken cancellationToken)
        {
            if (_cacheDataProvider.TryGetValue(CACHED_INVENTORIES_KEY, out List<InventoryModel> cachedInventories)
                &&
                cachedInventories != null && cachedInventories.Any())
            {
                return cachedInventories;
            }

            List<InventoryEntity> inventories = await _inventoryRepository.GetListAsync(cancellationToken);

            List<InventoryModel> mappedInventories =
                _mapper.Map<List<InventoryEntity>, List<InventoryModel>>(inventories);

            _cacheDataProvider.Set(CACHED_INVENTORIES_KEY, mappedInventories);

            return mappedInventories;
        }

        /// <summary>
        /// Yeni envanter oluşturur
        /// </summary>
        /// <param name="inventory">Oluşturulacak envanter nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateInventoryAsync(InventoryModel inventory, CancellationToken cancellationToken)
        {
            InventoryEntity mappedInventory = _mapper.Map<InventoryModel, InventoryEntity>(inventory);

            int createdInventoryId = await _inventoryRepository.CreateAsync(mappedInventory, cancellationToken);

            await CreateCheckpointAsync(
                rollback: new RollbackModel()
                {
                    TransactionType = TransactionType.Insert,
                    TransactionDate = DateTime.Now,
                    TransactionIdentity = "", // TODO: Bu alan request headerdan alınacak, header da yoksa burada GUID olarak oluşturulacak
                    RollbackItems = new List<RollbackItemModel>
                    {
                        new RollbackItemModel()
                        {
                            Identity = createdInventoryId,
                            DataSet = InventoryRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationToken: cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            inventory.Id = createdInventoryId;

            if (_cacheDataProvider.TryGetValue(CACHED_INVENTORIES_KEY, out List<InventoryModel> cachedInventories))
            {
                cachedInventories.Add(inventory);
                _cacheDataProvider.Set(CACHED_INVENTORIES_KEY, cachedInventories);
            }

            return createdInventoryId;
        }

        /// <summary>
        /// Yeni çalışan için varsayılan envanter ataması yapar
        /// </summary>
        /// <param name="inventory">Atanacak envanter</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<InventoryModel> CreateDefaultInventoryForNewWorkerAsync(InventoryModel inventory, CancellationToken cancellationToken)
        {
            List<InventoryModel> existingInventories = await GetInventoriesAsync(cancellationToken);

            if (!existingInventories.Any(x => x.Id == inventory.Id))
            {
                throw new Exception("Id ye ait envanter bulunamadı");
            }

            if (await _inventoryDefaultsRepository.CheckExistAsync(inventory.Id, cancellationToken))
            {
                throw new Exception("Bu envanter zaten atanmış");
            }

            await _inventoryDefaultsRepository.CreateAsync(
                 inventoryDefault: new InventoryDefaultsEntity()
                 {
                     InventoryId = inventory.Id,
                     ForNewWorker = true
                 },
                 cancellationToken: cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            if (_cacheDataProvider.TryGetValue(CACHED_INVENTORIES_DEFAULTS_KEY, out List<InventoryModel> cachedInventories)
                &&
                cachedInventories != null && cachedInventories.Any())
            {
                cachedInventories.Add(existingInventories.FirstOrDefault(x => x.Id == inventory.Id));

                _cacheDataProvider.Set(CACHED_INVENTORIES_DEFAULTS_KEY, cachedInventories);
            }

            return inventory;
        }

        /// <summary>
        /// Yeni çalışanlara verilecek envanterlerin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public List<InventoryModel> GetInventoriesForNewWorker(CancellationToken cancellationToken)
        {
            if (_cacheDataProvider.TryGetValue(CACHED_INVENTORIES_DEFAULTS_KEY, out List<InventoryModel> cachedInventories)
                &&
                cachedInventories != null && cachedInventories.Any())
            {
                return cachedInventories;
            }

            Task<List<InventoryEntity>> inventoriesTask = _inventoryRepository.GetListAsync(cancellationToken);
            Task<List<InventoryDefaultsEntity>> inventoryDefaultsTask = _inventoryDefaultsRepository.GetListAsync(cancellationToken);

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

            _cacheDataProvider.Set(CACHED_INVENTORIES_DEFAULTS_KEY, inventories, new TimeSpan(hours: 0, minutes: 10, seconds: 0));

            return inventories;
        }

        /// <summary>
        /// Bir çalışana envanter ataması yapar
        /// </summary>
        /// <param name="worker">Envanter bilgisini içeren çalışan nesnesi</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<WorkerModel> AssignInventoryToWorkerAsync(WorkerModel worker, CancellationToken cancellationToken)
        {
            List<int> inventoryIds = worker.AAInventories.Select(x => x.Id).ToList();

            List<InventoryEntity> inventories =
                await _inventoryRepository.GetForSpecificIdAsync(inventoryIds, cancellationToken);

            foreach (var inventoryId in inventoryIds)
            {
                if (!inventories.Select(x => x.Id).Contains(inventoryId))
                {
                    throw new Exception($"{inventoryId} Id değerine sahip envanter bulunamadı");
                }

                InventoryEntity inventoryEntity = inventories.FirstOrDefault(x => x.Id == inventoryId);

                if (inventoryEntity.CurrentStockCount <= 0)
                {
                    throw new Exception($"{inventoryEntity.Name} (Id:{inventoryEntity.Id}) için yetersiz stok");
                }
            }

            foreach (var inventoryModel in worker.AAInventories)
            {
                await _workerInventoryRepository.CreateAsync(new WorkerInventoryEntity
                {
                    FromDate = worker.FromDate,
                    ToDate = worker.ToDate,
                    InventoryId = inventoryModel.Id,
                    WorkerId = worker.Id
                }, cancellationToken);
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            return worker;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                    _inventoryRepository.Dispose();
                    _unitOfWork.Dispose();
                }

                disposed = true;
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
                if (rollbackItem.DataSet?.ToString() == InventoryRepository.TABLE_NAME)
                {
                    if (rollbackItem.RollbackType == RollbackType.Delete)
                    {
                        await _inventoryRepository.DeleteAsync((int)rollbackItem.Identity, cancellationToken);
                    }
                    else if (rollbackItem.RollbackType == RollbackType.Insert)
                    {
                        await _inventoryRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationToken);
                    }
                    else if (rollbackItem.RollbackType == RollbackType.Update)
                    {
                        await _inventoryRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationToken);
                    }
                }
            }

            return await _transactionRepository.SetRolledbackAsync(rollback.TransactionIdentity, cancellationToken);
        }
    }
}
