﻿using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Services.Business.Departments.Buying.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.Buying.Repositories.Sql;
using MicroserviceProject.Services.Model.Department.Buying;
using MicroserviceProject.Services.Transaction;
using MicroserviceProject.Services.Transaction.Models;
using MicroserviceProject.Services.Transaction.Types;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Buying.Services
{
    /// <summary>
    /// Talep işlemleri iş mantığı sınıfı
    /// </summary>
    public class RequestService : BaseService, IRollbackableAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "MicroserviceProject.Services.Business.Departments.Buying.Services.RequestService";

        /// <summary>
        /// Önbelleğe alınan envanter taleplerinin önbellekteki adı
        /// </summary>
        private const string CACHED_REQUESTS_KEY = "MicroserviceProject.Services.Business.Departments.Buying.InventoryRequests";

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
        /// Envanter talepleri tablosu için repository sınıfı
        /// </summary>
        private readonly InventoryRequestRepository _inventoryRequestRepository;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Talep işlemleri iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="cacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="transactionRepository">İşlem tablosu için repository sınıfı</param>
        /// <param name="transactionItemRepository">İşlem öğesi tablosu için repository sınıfı</param>
        /// <param name="inventoryRequestRepository">Envanter talepleri tablosu için repository sınıfı</param>
        public RequestService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            CacheDataProvider cacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            InventoryRequestRepository inventoryRequestRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cacheDataProvider = cacheDataProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _inventoryRequestRepository = inventoryRequestRepository;
        }

        /// <summary>
        /// Envanter taleplerinin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<InventoryRequestModel>> GetInventoryRequestsAsync(CancellationToken cancellationToken)
        {
            if (_cacheDataProvider.TryGetValue(CACHED_REQUESTS_KEY, out List<InventoryRequestModel> cachedInventoryRequests)
                &&
                cachedInventoryRequests != null && cachedInventoryRequests.Any())
            {
                return cachedInventoryRequests;
            }

            List<InventoryRequestEntity> inventoryRequests = await _inventoryRequestRepository.GetListAsync(cancellationToken);

            List<InventoryRequestModel> mappedInventoryRequests =
                _mapper.Map<List<InventoryRequestEntity>, List<InventoryRequestModel>>(inventoryRequests);

            _cacheDataProvider.Set(CACHED_REQUESTS_KEY, mappedInventoryRequests);

            return mappedInventoryRequests;
        }

        /// <summary>
        /// Yeni envanter talebi oluşturur
        /// </summary>
        /// <param name="inventoryRequest">Oluşturulacak envanter talebi nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateInventoryRequestAsync(InventoryRequestModel inventoryRequest, CancellationToken cancellationToken)
        {
            InventoryRequestEntity mappedInventoryRequest = _mapper.Map<InventoryRequestModel, InventoryRequestEntity>(inventoryRequest);

            int createdInventoryRequestId = await _inventoryRequestRepository.CreateAsync(mappedInventoryRequest, cancellationToken);

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
                            Identity = createdInventoryRequestId,
                            DataSet = InventoryRequestRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationToken: cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            inventoryRequest.Id = createdInventoryRequestId;

            if (_cacheDataProvider.TryGetValue(CACHED_REQUESTS_KEY, out List<InventoryRequestModel> cachedInventoryRequests))
            {
                cachedInventoryRequests.Add(inventoryRequest);
                _cacheDataProvider.Set(CACHED_REQUESTS_KEY, cachedInventoryRequests);
            }

            return createdInventoryRequestId;
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
                    _inventoryRequestRepository.Dispose();
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
                switch (rollbackItem.DataSet?.ToString())
                {
                    case InventoryRequestRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _inventoryRequestRepository.DeleteAsync((int)rollbackItem.Identity, cancellationToken);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _inventoryRequestRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationToken);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _inventoryRequestRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationToken);
                        }
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