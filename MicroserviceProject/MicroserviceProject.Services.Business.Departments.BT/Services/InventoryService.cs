using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Services.Business.Departments.IT.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.IT.Repositories.Sql;
using MicroserviceProject.Services.Model.Department.HR;
using MicroserviceProject.Services.Model.Department.IT;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.IT.Services
{
    /// <summary>
    /// Envanter işlemleri iş mantığı sınıfı
    /// </summary>
    public class InventoryService : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Önbelleğe alınan envanterlerin önbellekteki adı
        /// </summary>
        private const string CACHED_INVENTORIES_KEY = "MicroserviceProject.Services.Business.Departments.IT.Inventories";

        /// <summary>
        /// Önbelleğe alınan varsayılan envanterlerin önbellekteki adı
        /// </summary>
        private const string CACHED_INVENTORIES_DEFAULTS_KEY = "MicroserviceProject.Services.Business.Departments.IT.InventoryDefaults";

        /// <summary>
        /// Rediste tutulan önbellek yönetimini sağlayan sınıf
        /// </summary>
        private readonly CacheDataProvider _cacheDataProvider;

        /// <summary>
        /// Mapping işlemleri için mapper nesnesi
        /// </summary>
        private readonly IMapper _mapper;

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
        /// <param name="inventoryRepository">Envanter tablosu için repository sınıfı</param>
        /// <param name="inventoryDefaultsRepository">Varsayılan envanterler tablosu için repository sınıfı</param>
        /// <param name="workerInventoryRepository">Çalışan envanterleri tablosu için repository sınıfı</param>
        public InventoryService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            CacheDataProvider cacheDataProvider,
            InventoryRepository inventoryRepository,
            InventoryDefaultsRepository inventoryDefaultsRepository,
            WorkerInventoryRepository workerInventoryRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cacheDataProvider = cacheDataProvider;
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

            await _unitOfWork.SaveAsync(cancellationToken);

            inventory.Id = createdInventoryId;

            if (_cacheDataProvider.TryGetValue(CACHED_INVENTORIES_KEY, out List<InventoryModel> cachedInventories))
            {
                cachedInventories.Add(inventory);
                _cacheDataProvider.Set(CACHED_INVENTORIES_KEY, cachedInventories);
            }
            else
            {
                List<InventoryModel> inventories = await GetInventoriesAsync(cancellationToken);

                inventories.Add(inventory);

                _cacheDataProvider.Set(CACHED_INVENTORIES_KEY, inventories);
            }

            return createdInventoryId;
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
            List<int> inventoryIds = worker.ITInventories.Select(x => x.Id).ToList();

            List<InventoryEntity> inventories =
                await _inventoryRepository.GetForSpecificIdAsync(inventoryIds, cancellationToken);

            foreach (var inventoryId in inventoryIds)
            {
                if (!inventories.Select(x => x.Id).Contains(inventoryId))
                {
                    throw new Exception($"{inventoryId} Id değerine sahip envanter bulunamadı");
                }
            }

            foreach (var inventoryModel in worker.ITInventories)
            {
                inventoryModel.Id = await _workerInventoryRepository.CreateAsync(new WorkerInventoryEntity
                {
                    FromDate = inventoryModel.FromDate,
                    ToDate = inventoryModel.ToDate,
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
    }
}
