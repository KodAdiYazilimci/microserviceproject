using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Services.Business.Departments.IT.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.IT.Repositories.Sql;
using MicroserviceProject.Services.Business.Model.Department.IT;
using MicroserviceProject.Services.Business.Util.UnitOfWork;

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
        public InventoryService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            CacheDataProvider cacheDataProvider,
            InventoryRepository inventoryRepository)
        {
            _mapper = mapper;
            _cacheDataProvider = cacheDataProvider;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
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
