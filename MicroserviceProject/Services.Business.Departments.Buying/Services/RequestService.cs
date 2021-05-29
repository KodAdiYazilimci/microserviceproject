using AutoMapper;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Http.Broker.Exceptions;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Department.AA;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Department.Finance;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Department.IT;
using Infrastructure.Localization.Providers;
using Infrastructure.Routing.Providers;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork;

using Services.Business.Departments.Buying.Entities.Sql;
using Services.Business.Departments.Buying.Models;
using Services.Business.Departments.Buying.Repositories.Sql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Buying.Services
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
        public override string ServiceName => "Services.Business.Departments.Buying.Services.RequestService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Business.Departments.Buying";

        /// <summary>
        /// Önbelleğe alınan envanter taleplerinin önbellekteki adı
        /// </summary>
        private const string CACHED_REQUESTS_KEY = "Services.Business.Departments.Buying.InventoryRequests";

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
        /// Servislerin rota isimlerini sağlayan sınıf
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Diğer servislerle iletişim kuracak ara bulucu
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// İdari işler departmanına satın alımla ilgili olumlu veya olumsuz dönüş verisini rabbit kuyruğuna ekleyecek nesne
        /// </summary>
        private readonly AAInformInventoryRequestPublisher _AAInformInventoryRequestPublisher;

        /// <summary>
        /// Bilgi teknolojileri departmanına satın alımla ilgili olumlu veya olumsuz dönüş verisini rabbit kuyruğuna ekleyecek nesne
        /// </summary>
        private readonly ITInformInventoryRequestPublisher _ITInformInventoryRequestPublisher;

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
        /// <param name="routeNameProvider">Servislerin rota isimlerini sağlayan sınıf</param>
        /// <param name="serviceCommunicator">Diğer servislerle iletişim kuracak ara bulucu</param>
        /// <param name="redisCacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="transactionRepository">İşlem tablosu için repository sınıfı</param>
        /// <param name="transactionItemRepository">İşlem öğesi tablosu için repository sınıfı</param>
        /// <param name="inventoryRequestRepository">Envanter talepleri tablosu için repository sınıfı</param>
        /// <param name="aaInformInventoryRequestPublisher">İdari işler departmanına satın alımla ilgili olumlu veya olumsuz 
        /// dönüş verisini rabbit kuyruğuna ekleyecek nesne</param>
        /// <param name="itInformInventoryRequestPublisher">Bilgi teknolojileri departmanına satın alımla ilgili olumlu veya 
        /// olumsuz dönüş verisini rabbit kuyruğuna ekleyecek nesne</param>
        public RequestService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            TranslationProvider translationProvider,
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator,
            RedisCacheDataProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            InventoryRequestRepository inventoryRequestRepository,
            AAInformInventoryRequestPublisher aaInformInventoryRequestPublisher,
            ITInformInventoryRequestPublisher itInformInventoryRequestPublisher,
            InventoryRequestPublisher inventoryRequestPublisher)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;
            _redisCacheDataProvider = redisCacheDataProvider;
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _inventoryRequestRepository = inventoryRequestRepository;

            _AAInformInventoryRequestPublisher = aaInformInventoryRequestPublisher;
            _ITInformInventoryRequestPublisher = itInformInventoryRequestPublisher;
            _inventoryRequestPublisher = inventoryRequestPublisher;
        }

        /// <summary>
        /// Envanter taleplerinin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
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

            List<InventoryModel> aaInventories = new List<InventoryModel>();

            if (mappedInventoryRequests.Any(x => x.DepartmentId == (int)Constants.Departments.AdministrativeAffairs))
            {
                aaInventories = await GetAAInventoriesAsync(cancellationTokenSource);
            }

            List<InventoryModel> itInventories = new List<InventoryModel>();

            if (mappedInventoryRequests.Any(x => x.DepartmentId == (int)Constants.Departments.AdministrativeAffairs))
            {
                itInventories = await GetITInventoriesAsync(cancellationTokenSource);
            }

            foreach (var requestModel in mappedInventoryRequests)
            {
                if (requestModel.DepartmentId == (int)Constants.Departments.AdministrativeAffairs)
                {
                    requestModel.AAInventory = aaInventories.FirstOrDefault(x => x.Id == requestModel.InventoryId);
                }
                else if (requestModel.DepartmentId == (int)Constants.Departments.InformationTechnologies)
                {
                    requestModel.ITInventory = itInventories.FirstOrDefault(x => x.Id == requestModel.InventoryId);
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
        private async Task<List<InventoryModel>> GetAAInventoriesAsync(CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<InventoryModel>> serviceResult =
                    await
                    _serviceCommunicator.Call<List<InventoryModel>>(
                            serviceName: _routeNameProvider.AA_GetInventories,
                            postData: null,
                            queryParameters: null,
                            headers: new List<KeyValuePair<string, string>>()
                            {
                                new KeyValuePair<string, string>("TransactionIdentity", TransactionIdentity)
                            },
                            cancellationTokenSource: cancellationTokenSource);

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

            return serviceResult.Data;
        }

        /// <summary>
        /// IT departmanına ait envanterlerin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        private async Task<List<InventoryModel>> GetITInventoriesAsync(CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<InventoryModel>> serviceResult =
                    await
                    _serviceCommunicator.Call<List<InventoryModel>>(
                            serviceName: _routeNameProvider.IT_GetInventories,
                            postData: null,
                            queryParameters: null,
                            headers: new List<KeyValuePair<string, string>>()
                            {
                                new KeyValuePair<string, string>("TransactionIdentity", TransactionIdentity)
                            },
                            cancellationTokenSource: cancellationTokenSource);

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

            return serviceResult.Data;
        }

        /// <summary>
        /// Yeni envanter talebi oluşturur
        /// </summary>
        /// <param name="inventoryRequest">Oluşturulacak envanter talebi nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateInventoryRequestAsync(InventoryRequestModel inventoryRequest, CancellationTokenSource cancellationTokenSource)
        {
            InventoryRequestEntity mappedInventoryRequest = _mapper.Map<InventoryRequestModel, InventoryRequestEntity>(inventoryRequest);

            if (mappedInventoryRequest.DepartmentId == (int)Constants.Departments.AdministrativeAffairs)
            {
                List<InventoryModel> aaInventories = await GetAAInventoriesAsync(cancellationTokenSource);

                if (!aaInventories.Any(x => x.Id == mappedInventoryRequest.InventoryId))
                {
                    throw new Exception("Envanter Id si bulunamadı");
                }
            }
            else if (mappedInventoryRequest.DepartmentId == (int)Constants.Departments.InformationTechnologies)
            {
                List<InventoryModel> itInventories = await GetITInventoriesAsync(cancellationTokenSource);

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
                model: new Infrastructure.Communication.Mq.Rabbit.Publisher.Department.Finance.Models.DecidedCostModel
                {
                    InventoryRequestId = createdInventoryRequestId
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
                    model: new Infrastructure.Communication.Mq.Rabbit.Publisher.Department.AA.Models.InventoryRequestModel
                    {
                        InventoryId = inventoryRequestEntity.InventoryId,
                        Amount = inventoryRequestEntity.Amount,
                        Revoked = decidedCost.Approved,
                        Done = true
                    });
            }
            else if (inventoryRequestEntity.DepartmentId == (int)Constants.Departments.InformationTechnologies)
            {
                _ITInformInventoryRequestPublisher.AddToBuffer(
                    model: new Infrastructure.Communication.Mq.Rabbit.Publisher.Department.IT.Models.InventoryRequestModel
                    {
                        InventoryId = inventoryRequestEntity.InventoryId,
                        Amount = inventoryRequestEntity.Amount,
                        Revoked = decidedCost.Approved,
                        Done = true
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
        }
    }
}
