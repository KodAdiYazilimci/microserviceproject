using AutoMapper;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Localization.Translation.Provider;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.Buying.Entities.Sql;
using Services.Api.Business.Departments.Buying.Repositories.Sql;
using Services.Communication.Http.Broker.Department.AA;
using Services.Communication.Http.Broker.Department.Buying.Models;
using Services.Communication.Http.Broker.Department.IT;
using Services.Communication.Mq.Queue.Finance.Models;
using Services.Communication.Mq.Queue.Finance.Rabbit.Publishers;
using Services.Logging.Aspect.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Services
{
    /// <summary>
    /// Talep işlemleri iş mantığı sınıfı
    /// </summary>
    public class RequestService : BaseService, IRollbackableAsync<int>, IDisposable, IDisposableInjections
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Api.Business.Departments.Buying.Services.RequestService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Api.Business.Departments.Buying";

        /// <summary>
        /// Önbelleğe alınan envanter taleplerinin önbellekteki adı
        /// </summary>
        private const string CACHED_REQUESTS_KEY = "Services.Api.Business.Departments.Buying.InventoryRequests";

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
        /// Envanter talepleri tablosu için repository sınıfı
        /// </summary>
        private readonly InventoryRequestRepository _inventoryRequestRepository;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

        /// <summary>
        /// İdari işler servis iletişimcisi
        /// </summary>
        private readonly Communication.Http.Broker.Department.AA.AACommunicator _aaCommunicator;

        /// <summary>
        /// IT departmanı servis iletişimcisi
        /// </summary>
        private readonly Communication.Http.Broker.Department.IT.ITCommunicator _itCommunicator;

        /// <summary>
        /// İdari işler departmanına satın alımla ilgili olumlu veya olumsuz dönüş verisini rabbit kuyruğuna ekleyecek nesne
        /// </summary>
        private readonly Communication.Mq.Queue.AA.Rabbit.Publishers.InformInventoryRequestPublisher _AAInformInventoryRequestPublisher;

        /// <summary>
        /// Bilgi teknolojileri departmanına satın alımla ilgili olumlu veya olumsuz dönüş verisini rabbit kuyruğuna ekleyecek nesne
        /// </summary>
        private readonly Communication.Mq.Queue.IT.Rabbit.Publishers.InformInventoryRequestPublisher _ITInformInventoryRequestPublisher;

        /// <summary>
        /// Satınalma departmanından alınması istenilen envanter talepleri için kayıt açan nesne
        /// </summary>
        protected readonly InventoryRequestPublisher _inventoryRequestPublisher;

        /// <summary>
        /// Talep işlemleri iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="translationProvider">Dil çeviri sağlayıcısı sınıf</param>        
        /// <param name="redisCacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="transactionRepository">İşlem tablosu için repository sınıfı</param>
        /// <param name="transactionItemRepository">İşlem öğesi tablosu için repository sınıfı</param>
        /// <param name="inventoryRequestRepository">Envanter talepleri tablosu için repository sınıfı</param>
        /// <param name="aaCommunicator">İdari işler servis iletişimcisi</param>
        /// <param name="itCommunicator">IT departmanı servis iletişimcisi</param>
        /// <param name="aaInformInventoryRequestPublisher">İdari işler departmanına satın alımla ilgili olumlu veya olumsuz 
        /// dönüş verisini rabbit kuyruğuna ekleyecek nesne</param>
        /// <param name="itInformInventoryRequestPublisher">Bilgi teknolojileri departmanına satın alımla ilgili olumlu veya 
        /// olumsuz dönüş verisini rabbit kuyruğuna ekleyecek nesne</param>
        /// <param name="inventoryRequestPublisher">Satınalma departmanından alınması istenilen envanter talepleri için kayıt açan nesne</param>
        public RequestService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            TranslationProvider translationProvider,
            RedisCacheDataProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            InventoryRequestRepository inventoryRequestRepository,
            AACommunicator aaCommunicator,
            ITCommunicator itCommunicator,
            Communication.Mq.Queue.AA.Rabbit.Publishers.InformInventoryRequestPublisher aaInformInventoryRequestPublisher,
            Communication.Mq.Queue.IT.Rabbit.Publishers.InformInventoryRequestPublisher itInformInventoryRequestPublisher,
            InventoryRequestPublisher inventoryRequestPublisher)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;
            _redisCacheDataProvider = redisCacheDataProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _inventoryRequestRepository = inventoryRequestRepository;

            _aaCommunicator = aaCommunicator;
            _itCommunicator = itCommunicator;

            _AAInformInventoryRequestPublisher = aaInformInventoryRequestPublisher;
            _ITInformInventoryRequestPublisher = itInformInventoryRequestPublisher;
            _inventoryRequestPublisher = inventoryRequestPublisher;
        }

        /// <summary>
        /// Envanter taleplerinin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(GetInventoryRequestsAsync))]
        [LogAfterRuntimeAttr(nameof(GetInventoryRequestsAsync))]
        public async Task<List<InventoryRequestModel>> GetInventoryRequestsAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_REQUESTS_KEY, out List<InventoryRequestModel> cachedInventoryRequests)
                &&
                cachedInventoryRequests != null && cachedInventoryRequests.Any())
            {
                return cachedInventoryRequests;
            }

            List<InventoryRequestEntity> inventoryRequests = await _inventoryRequestRepository.GetListAsync(cancellationTokenSource);

            List<InventoryRequestModel> mappedInventoryRequests =
                _mapper.Map<List<InventoryRequestEntity>, List<InventoryRequestModel>>(inventoryRequests);

            List<Communication.Http.Broker.Department.AA.Models.InventoryModel> aaInventories = new List<Communication.Http.Broker.Department.AA.Models.InventoryModel>();

            if (mappedInventoryRequests.Any(x => x.DepartmentId == (int)Constants.Departments.AdministrativeAffairs))
            {
                aaInventories = await GetAAInventoriesAsync(cancellationTokenSource);
            }

            List<Communication.Http.Broker.Department.IT.Models.InventoryModel> itInventories = new List<Communication.Http.Broker.Department.IT.Models.InventoryModel>();

            if (mappedInventoryRequests.Any(x => x.DepartmentId == (int)Constants.Departments.AdministrativeAffairs))
            {
                itInventories = await GetITInventoriesAsync(cancellationTokenSource);
            }

            foreach (var requestModel in mappedInventoryRequests)
            {
                if (requestModel.DepartmentId == (int)Constants.Departments.AdministrativeAffairs)
                {
                    if (aaInventories.Any(x => x.Id == requestModel.InventoryId))
                    {
                        Communication.Http.Broker.Department.AA.Models.InventoryModel inventory = aaInventories.FirstOrDefault(x => x.Id == requestModel.InventoryId);

                        requestModel.AAInventory = new InventoryModel()
                        {
                            CurrentStockCount = inventory.CurrentStockCount,
                            FromDate = inventory.FromDate,
                            Id = inventory.Id,
                            Name = inventory.Name,
                            ToDate = inventory.ToDate
                        };
                    }
                }
                else if (requestModel.DepartmentId == (int)Constants.Departments.InformationTechnologies)
                {
                    if (itInventories.Any(x => x.Id == requestModel.InventoryId))
                    {
                        Communication.Http.Broker.Department.IT.Models.InventoryModel inventory = itInventories.FirstOrDefault(x => x.Id == requestModel.InventoryId);

                        requestModel.ITInventory = new InventoryModel()
                        {
                            CurrentStockCount = inventory.CurrentStockCount,
                            FromDate = inventory.FromDate,
                            ToDate = inventory.ToDate,
                            Name = inventory.Name,
                            Id = inventory.Id
                        };
                    }
                }
                else
                    throw new Exception("Tanımlanmamış departman Id si");
            }

            _redisCacheDataProvider.Set(CACHED_REQUESTS_KEY, mappedInventoryRequests);

            return mappedInventoryRequests;
        }

        /// <summary>
        /// İdari işler departmanına ait envanterlerin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(GetAAInventoriesAsync))]
        [LogAfterRuntimeAttr(nameof(GetAAInventoriesAsync))]
        private async Task<List<Communication.Http.Broker.Department.AA.Models.InventoryModel>> GetAAInventoriesAsync(CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<Communication.Http.Broker.Department.AA.CQRS.Queries.Responses.GetInventoriesQueryResponse> serviceResult =
                await _aaCommunicator.GetInventoriesAsync(TransactionIdentity, cancellationTokenSource);

            if (!serviceResult.IsSuccess)
            {
                throw new CallException(
                        message: serviceResult.ErrorModel.Description,
                        endpoint:
                        !string.IsNullOrEmpty(serviceResult.SourceApiService)
                        ?
                        serviceResult.SourceApiService
                        :
                        $"{ApiServiceName}).{nameof(RequestService)}.{nameof(GetAAInventoriesAsync)}",
                        error: serviceResult.ErrorModel,
                        validation: serviceResult.Validation);
            }

            return serviceResult.Data.Inventories;
        }

        /// <summary>
        /// IT departmanına ait envanterlerin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(GetITInventoriesAsync))]
        [LogAfterRuntimeAttr(nameof(GetITInventoriesAsync))]
        private async Task<List<Communication.Http.Broker.Department.IT.Models.InventoryModel>> GetITInventoriesAsync(CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<Communication.Http.Broker.Department.IT.CQRS.Queries.Responses.GetInventoriesQueryResponse> serviceResult =
                await _itCommunicator.GetInventoriesAsync(TransactionIdentity, cancellationTokenSource);

            if (!serviceResult.IsSuccess)
            {
                throw new CallException(
                        message: serviceResult.ErrorModel.Description,
                        endpoint:
                        !string.IsNullOrEmpty(serviceResult.SourceApiService)
                        ?
                        serviceResult.SourceApiService
                        :
                        $"{ApiServiceName}).{nameof(RequestService)}.{nameof(GetAAInventoriesAsync)}",
                        error: serviceResult.ErrorModel,
                        validation: serviceResult.Validation);
            }

            return serviceResult.Data.Inventories;
        }

        /// <summary>
        /// Yeni envanter talebi oluşturur
        /// </summary>
        /// <param name="inventoryRequest">Oluşturulacak envanter talebi nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(CreateInventoryRequestAsync))]
        [LogAfterRuntimeAttr(nameof(CreateInventoryRequestAsync))]
        public async Task<int> CreateInventoryRequestAsync(InventoryRequestModel inventoryRequest, CancellationTokenSource cancellationTokenSource)
        {
            InventoryRequestEntity mappedInventoryRequest = _mapper.Map<InventoryRequestModel, InventoryRequestEntity>(inventoryRequest);

            if (mappedInventoryRequest.DepartmentId == (int)Constants.Departments.AdministrativeAffairs)
            {
                List<Communication.Http.Broker.Department.AA.Models.InventoryModel> aaInventories = await GetAAInventoriesAsync(cancellationTokenSource);

                if (!aaInventories.Any(x => x.Id == mappedInventoryRequest.InventoryId))
                {
                    throw new Exception("Envanter Id si bulunamadı");
                }
            }
            else if (mappedInventoryRequest.DepartmentId == (int)Constants.Departments.InformationTechnologies)
            {
                List<Communication.Http.Broker.Department.IT.Models.InventoryModel> itInventories = await GetITInventoriesAsync(cancellationTokenSource);

                if (!itInventories.Any(x => x.Id == mappedInventoryRequest.InventoryId))
                {
                    throw new Exception("Envanter Id si bulunamadı");
                }
            }
            else
                throw new Exception("Tanımlanmamış departman Id si");

            int createdInventoryRequestId = await _inventoryRequestRepository.CreateAsync(mappedInventoryRequest, cancellationTokenSource);

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
                            Identity = createdInventoryRequestId,
                            DataSet = InventoryRequestRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            _inventoryRequestPublisher.AddToBuffer(
                model: new DecidedCostQueueModel
                {
                    InventoryRequestId = createdInventoryRequestId,
                    TransactionIdentity = TransactionIdentity,
                    GeneratedBy = ApiServiceName
                });

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            await _inventoryRequestPublisher.PublishBufferAsync(cancellationTokenSource);

            inventoryRequest.Id = createdInventoryRequestId;

            if (_redisCacheDataProvider.TryGetValue(CACHED_REQUESTS_KEY, out List<InventoryRequestModel> cachedInventoryRequests)
                &&
                cachedInventoryRequests != null)
            {
                cachedInventoryRequests.Add(inventoryRequest);

                _redisCacheDataProvider.Set(CACHED_REQUESTS_KEY, cachedInventoryRequests);
            }

            return createdInventoryRequestId;
        }

        /// <summary>
        /// Satın alınması planlanan envantere ait masrafın durumununu sonuçlandırır
        /// </summary>
        /// <param name="decidedCost">Satın alım kararına ait modelin nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(ValidateCostInventoryAsync))]
        [LogAfterRuntimeAttr(nameof(ValidateCostInventoryAsync))]
        public async Task<int> ValidateCostInventoryAsync(DecidedCostModel decidedCost, CancellationTokenSource cancellationTokenSource)
        {
            int result;

            if (decidedCost.Approved)
            {
                await
                    CreateCheckpointAsync(
                        rollback: new RollbackModel()
                        {
                            TransactionDate = DateTime.Now,
                            TransactionIdentity = TransactionIdentity,
                            TransactionType = TransactionType.Update,
                            RollbackItems = new List<RollbackItemModel>
                            {
                                new RollbackItemModel
                                {
                                    Identity = decidedCost.InventoryRequestId,
                                    DataSet = InventoryRequestRepository.TABLE_NAME,
                                    Name = nameof(InventoryRequestEntity.Revoked),
                                    NewValue = true,
                                    OldValue = false,
                                    RollbackType = RollbackType.Update
                                },
                                new RollbackItemModel
                                {
                                    Identity = decidedCost.InventoryRequestId,
                                    DataSet = InventoryRequestRepository.TABLE_NAME,
                                    Name = nameof(InventoryRequestEntity.Done),
                                    NewValue = true,
                                    OldValue = false,
                                    RollbackType = RollbackType.Update
                                }
                            }
                        },
                        cancellationTokenSource: cancellationTokenSource);

                result = await _inventoryRequestRepository.RevokeAsync(decidedCost.InventoryRequestId, cancellationTokenSource);
            }
            else
            {
                await
                    CreateCheckpointAsync(
                        rollback: new RollbackModel()
                        {
                            TransactionDate = DateTime.Now,
                            TransactionIdentity = TransactionIdentity,
                            TransactionType = TransactionType.Update,
                            RollbackItems = new List<RollbackItemModel>
                            {
                                new RollbackItemModel
                                {
                                    Identity = decidedCost.InventoryRequestId,
                                    DataSet = InventoryRequestRepository.TABLE_NAME,
                                    Name = nameof(InventoryRequestEntity.Revoked),
                                    NewValue = false,
                                    OldValue = false,
                                    RollbackType = RollbackType.Update
                                },
                                new RollbackItemModel
                                {
                                    Identity = decidedCost.InventoryRequestId,
                                    DataSet = InventoryRequestRepository.TABLE_NAME,
                                    Name = nameof(InventoryRequestEntity.Done),
                                    NewValue = true,
                                    OldValue = false,
                                    RollbackType = RollbackType.Update
                                }
                            }
                        },
                        cancellationTokenSource: cancellationTokenSource);

                result = await _inventoryRequestRepository.UnRevokeAsync(decidedCost.InventoryRequestId, cancellationTokenSource);
            }

            InventoryRequestEntity inventoryRequestEntity = await _inventoryRequestRepository.GetAsync(decidedCost.InventoryRequestId, cancellationTokenSource);

            if (inventoryRequestEntity == null)
            {
                throw new Exception("Envanter talebi bulunamadı");
            }

            if (inventoryRequestEntity.DepartmentId == (int)Constants.Departments.AdministrativeAffairs)
            {
                _AAInformInventoryRequestPublisher.AddToBuffer(
                    model: new Communication.Mq.Queue.AA.Models.InventoryRequestQueueModel
                    {
                        InventoryId = inventoryRequestEntity.InventoryId,
                        Amount = inventoryRequestEntity.Amount,
                        Revoked = decidedCost.Approved,
                        Done = true,
                        TransactionIdentity = TransactionIdentity,
                        GeneratedBy = ApiServiceName
                    });
            }
            else if (inventoryRequestEntity.DepartmentId == (int)Constants.Departments.InformationTechnologies)
            {
                _ITInformInventoryRequestPublisher.AddToBuffer(
                    model: new Communication.Mq.Queue.IT.Models.InventoryRequestQueueModel
                    {
                        InventoryId = inventoryRequestEntity.InventoryId,
                        Amount = inventoryRequestEntity.Amount,
                        Revoked = decidedCost.Approved,
                        Done = true,
                        TransactionIdentity = TransactionIdentity,
                        GeneratedBy = ApiServiceName
                    });
            }

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            Task.WaitAll(new Task[]
            {
                _AAInformInventoryRequestPublisher.PublishBufferAsync(cancellationTokenSource),
                _ITInformInventoryRequestPublisher.PublishBufferAsync(cancellationTokenSource)
            }, cancellationTokenSource.Token);

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
        [LogBeforeRuntimeAttr(nameof(CreateCheckpointAsync))]
        [LogAfterRuntimeAttr(nameof(CreateCheckpointAsync))]
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
        [LogBeforeRuntimeAttr(nameof(GetProductionRequestsAsync))]
        [LogAfterRuntimeAttr(nameof(GetProductionRequestsAsync))]
        public async Task<int> GetProductionRequestsAsync(RollbackModel rollback, CancellationTokenSource cancellationTokenSource)
        {
            foreach (var rollbackItem in rollback.RollbackItems)
            {
                switch (rollbackItem.DataSet?.ToString())
                {
                    case InventoryRequestRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _inventoryRequestRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _inventoryRequestRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _inventoryRequestRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
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
            _inventoryRequestRepository.Dispose();
            _transactionItemRepository.Dispose();
            _transactionRepository.Dispose();
            _unitOfWork.Dispose();
            _AAInformInventoryRequestPublisher.Dispose();
            _ITInformInventoryRequestPublisher.Dispose();
            _translationProvider.Dispose();
            _aaCommunicator.Dispose();
            _itCommunicator.Dispose();
        }
    }
}
