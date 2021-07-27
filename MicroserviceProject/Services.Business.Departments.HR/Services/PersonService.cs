using AutoMapper;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Http.Broker.Exceptions;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Department.AA;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Department.Accounting;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Department.IT;
using Infrastructure.Localization.Providers;
using Infrastructure.Routing.Providers;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Business.Departments.HR.Entities.Sql;
using Services.Business.Departments.HR.Models;
using Services.Business.Departments.HR.Repositories.Sql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.HR.Services
{
    /// <summary>
    /// Kişi işlemleri iş mantığı sınıfı
    /// </summary>
    public class PersonService : BaseService, IRollbackableAsync<int>, IDisposable, IDisposableInjections
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İşlem sürecinde adı geçecek modül adı
        /// </summary>
        public override string ServiceName => "Services.Business.Departments.HR.Services.PersonService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Business.Departments.HR";

        /// <summary>
        /// Önbelleğe alınan kişilerin önbellekteki adı
        /// </summary>
        private const string CACHED_PEOPLE_KEY = "Services.Business.Departments.HR.People";
        private const string CACHED_WORKERS_KEY = "Services.Business.Departments.HR.Workers";
        private const string CACHED_TITLES_KEY = "Services.Business.Departments.HR.Titles";

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
        /// Servislerin rota isimlerini sağlayan sınıf
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Diğer servislerle iletişim kuracak ara bulucu
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

        /// <summary>
        /// Departmanlar tablosu için repository sınıfı
        /// </summary>
        private readonly DepartmentRepository _departmentRepository;

        /// <summary>
        /// Kişi tablosu için repository sınıfı
        /// </summary>
        private readonly PersonRepository _personRepository;

        /// <summary>
        /// Ünvan tablosu için repository sınıfı
        /// </summary>
        private readonly TitleRepository _titleRepository;

        /// <summary>
        /// Çalışan tablosu için repository sınıfı
        /// </summary>
        private readonly WorkerRepository _workerRepository;

        /// <summary>
        /// Çalışan ilişkileri tablosu için repository sınıfı
        /// </summary>
        private readonly WorkerRelationRepository _workerRelationRepository;

        /// <summary>
        /// İdari işler tarafından yeni çalışana varsayılan envanter ataması yapacak kuyruğa
        /// kayıt ekleyecek nesne
        /// </summary>
        private readonly AAAssignInventoryToWorkerPublisher _AAassignInventoryToWorkerPublisher;

        /// <summary>
        /// IT tarafından yeni çalışana varsayılan envanter ataması yapacak kuyruğa kayıt ekleyecek nesne
        /// </summary>
        private readonly ITAssignInventoryToWorkerPublisher _ITAssignInventoryToWorkerPublisher;

        /// <summary>
        /// Muhasebe tarafından yeni çalışana maaş hesabı açacak kuyruğa kayıt ekleyecek nesne
        /// </summary>
        private readonly CreateBankAccountPublisher _createBankAccountPublisher;

        /// <summary>
        /// Kişi işlemleri iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="routeNameProvider">Servislerin rota isimlerini sağlayan sınıf</param>
        /// <param name="serviceCommunicator">Diğer servislerle iletişim kuracak ara bulucu</param>
        /// <param name="AAassignInventoryToWorkerPublisher">İdari işler tarafından yeni çalışana 
        /// varsayılan envanter ataması yapacak kuyruğa kayıt ekleyecek nesne</param>
        /// <param name="ITassignInventoryToWorkerPublisher">IT tarafından yeni çalışana varsayılan envanter 
        /// ataması yapacak kuyruğa kayıt ekleyecek nesne</param>
        /// <param name="createBankAccountPublisher">Muhasebe tarafından yeni çalışana maaş hesabı açacak 
        /// kuyruğa kayıt ekleyecek nesne</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="translationProvider">Dil çeviri sağlayıcısı sınıf</param>
        /// <param name="redisCacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="departmentRepository">Departmanlar tablosu için repository sınıfı</param>
        /// <param name="personRepository">Kişi tablosu için repository sınıfı</param>
        /// <param name="titleRepository">Kişi tablosu için repository sınıfı</param>
        /// <param name="workerRepository">Çalışan tablosu için repository sınıfı</param>
        /// <param name="workerRelationRepository">Çalışan ilişkileri tablosu için repository sınıfı</param>
        public PersonService(
            IMapper mapper,
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator,
            AAAssignInventoryToWorkerPublisher AAassignInventoryToWorkerPublisher,
            ITAssignInventoryToWorkerPublisher ITassignInventoryToWorkerPublisher,
            CreateBankAccountPublisher createBankAccountPublisher,
            IUnitOfWork unitOfWork,
            TranslationProvider translationProvider,
            RedisCacheDataProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            DepartmentRepository departmentRepository,
            PersonRepository personRepository,
            TitleRepository titleRepository,
            WorkerRepository workerRepository,
            WorkerRelationRepository workerRelationRepository)
        {
            _redisCacheDataProvider = redisCacheDataProvider;
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;
            _AAassignInventoryToWorkerPublisher = AAassignInventoryToWorkerPublisher;
            _ITAssignInventoryToWorkerPublisher = ITassignInventoryToWorkerPublisher;
            _createBankAccountPublisher = createBankAccountPublisher;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _departmentRepository = departmentRepository;
            _personRepository = personRepository;
            _titleRepository = titleRepository;
            _workerRepository = workerRepository;
            _workerRelationRepository = workerRelationRepository;
        }

        /// <summary>
        /// Kişilerin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<PersonModel>> GetPeopleAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_PEOPLE_KEY, out List<PersonModel> cachedPeople)
                &&
                cachedPeople != null && cachedPeople.Any())
            {
                return cachedPeople;
            }

            List<PersonEntity> people = await _personRepository.GetListAsync(cancellationTokenSource);

            List<PersonModel> mappedPeople =
                _mapper.Map<List<PersonEntity>, List<PersonModel>>(people);

            _redisCacheDataProvider.Set(CACHED_PEOPLE_KEY, mappedPeople);

            return mappedPeople;
        }

        /// <summary>
        /// Yeni kişi oluşturur
        /// </summary>
        /// <param name="person">Oluşturulacak kişi nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreatePersonAsync(PersonModel person, CancellationTokenSource cancellationTokenSource)
        {
            PersonEntity mappedPerson = _mapper.Map<PersonModel, PersonEntity>(person);

            int createdPersonId = await _personRepository.CreateAsync(mappedPerson, cancellationTokenSource);

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
                            Identity = createdPersonId,
                            DataSet = PersonRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            person.Id = createdPersonId;

            if (_redisCacheDataProvider.TryGetValue(CACHED_PEOPLE_KEY, out List<PersonModel> cachedPeople) && cachedPeople != null)
            {
                cachedPeople.Add(person);

                _redisCacheDataProvider.Set(CACHED_PEOPLE_KEY, cachedPeople);
            }

            return createdPersonId;
        }

        /// <summary>
        /// Ünvanların listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<TitleModel>> GetTitlesAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_TITLES_KEY, out List<TitleModel> cachedTitles)
                &&
                cachedTitles != null && cachedTitles.Any())
            {
                return cachedTitles;
            }

            List<TitleEntity> titles = await _titleRepository.GetListAsync(cancellationTokenSource);

            List<TitleModel> mappedTitles =
                _mapper.Map<List<TitleEntity>, List<TitleModel>>(titles);

            _redisCacheDataProvider.Set(CACHED_TITLES_KEY, mappedTitles);

            return mappedTitles;
        }

        /// <summary>
        /// Yeni ünvan oluşturur
        /// </summary>
        /// <param name="title">Oluşturulacak ünvan nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateTitleAsync(TitleModel title, CancellationTokenSource cancellationTokenSource)
        {
            TitleEntity mappedTitles = _mapper.Map<TitleModel, TitleEntity>(title);

            int createdTitleId = await _titleRepository.CreateAsync(mappedTitles, cancellationTokenSource);

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
                            Identity = createdTitleId,
                            DataSet = TitleRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            title.Id = createdTitleId;

            if (_redisCacheDataProvider.TryGetValue(CACHED_TITLES_KEY, out List<TitleModel> cachedTitles) && cachedTitles != null)
            {
                cachedTitles.Add(title);

                _redisCacheDataProvider.Set(CACHED_TITLES_KEY, cachedTitles);
            }

            return createdTitleId;
        }

        /// <summary>
        /// Çalışanların listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<WorkerModel>> GetWorkersAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_WORKERS_KEY, out List<WorkerModel> cachedWorkers)
                &&
                cachedWorkers != null && cachedWorkers.Any())
            {
                return cachedWorkers;
            }

            List<WorkerModel> workerModels = GetWorkers(cancellationTokenSource);

            foreach (var worker in workerModels)
            {
                ServiceResultModel<List<BankAccountModel>> bankAccountsServiceResult =
                 await _serviceCommunicator.Call<List<BankAccountModel>>(
                     serviceName: _routeNameProvider.Accounting_GetBankAccountsOfWorker,
                     postData: null,
                     queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("workerId", worker.Id.ToString()) },
                     headers: new List<KeyValuePair<string, string>>()
                     {
                         new KeyValuePair<string, string>("TransactionIdentity", TransactionIdentity)
                     },
                     cancellationTokenSource: cancellationTokenSource);

                if (bankAccountsServiceResult.IsSuccess)
                {
                    worker.BankAccounts = bankAccountsServiceResult.Data;
                }
                else
                {
                    throw new CallException(
                        message: bankAccountsServiceResult.ErrorModel.Description,
                        endpoint:
                        !string.IsNullOrEmpty(bankAccountsServiceResult.SourceApiService)
                        ?
                        bankAccountsServiceResult.SourceApiService
                        :
                        $"{ApiServiceName}).{nameof(PersonService)}.{nameof(GetWorkersAsync)}",
                        error: bankAccountsServiceResult.ErrorModel,
                        validation: bankAccountsServiceResult.Validation);
                }
            }

            _redisCacheDataProvider.Set(CACHED_WORKERS_KEY, workerModels);

            return workerModels;
        }

        private List<WorkerModel> GetWorkers(CancellationTokenSource cancellationTokenSource)
        {
            //TO DO: Join sorgusu yazılacak

            Task<List<DepartmentEntity>> departmentTask = _departmentRepository.GetListAsync(cancellationTokenSource);

            Task<List<PersonEntity>> personTask = _personRepository.GetListAsync(cancellationTokenSource);

            Task<List<TitleEntity>> titleTask = _titleRepository.GetListAsync(cancellationTokenSource);

            Task<List<WorkerEntity>> workerTask = _workerRepository.GetListAsync(cancellationTokenSource);

            Task<List<WorkerRelationEntity>> workerRelationTask = _workerRelationRepository.GetListAsync(cancellationTokenSource);

            Task.WaitAll(new Task[] { departmentTask, personTask, titleTask, workerTask, workerRelationTask }, cancellationTokenSource.Token);

            var workerModels = (from w in workerTask.Result
                                join t in titleTask.Result
                                on w.TitleId equals t.Id
                                join p in personTask.Result
                                on w.PersonId equals p.Id
                                select new WorkerModel
                                {
                                    Id = w.Id,
                                    Department = _mapper.Map<DepartmentModel>(departmentTask.Result.FirstOrDefault(x => x.Id == w.DepartmentId)),
                                    Title = _mapper.Map<TitleModel>(titleTask.Result.FirstOrDefault(x => x.Id == w.TitleId)),
                                    Person = _mapper.Map<PersonModel>(p),
                                    FromDate = w.FromDate,
                                    ToDate = w.ToDate
                                }).ToList();
            return workerModels;
        }

        /// <summary>
        /// Yeni çalışan oluşturur
        /// </summary>
        /// <param name="worker">Oluşturulacak çalışan nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateWorkerAsync(WorkerModel worker, CancellationTokenSource cancellationTokenSource)
        {
            WorkerEntity mappedWorker = _mapper.Map<WorkerModel, WorkerEntity>(worker);

            worker.Id = await _workerRepository.CreateAsync(mappedWorker, cancellationTokenSource);

            // Not: Burada doğrudan diğer servislerle de iletişime geçilebilir 
            // veya rabbit kuyruğuna kayıt atılabilir

            #region Muhasebe departmanının banka hesabı açması için rabbit e kayıt ekler

            _createBankAccountPublisher.AddToBuffer(
                model: new Infrastructure.Communication.Mq.Rabbit.Publisher.Department.Accounting.Models.BankAccountModel()
                {
                    Worker = new Infrastructure.Communication.Mq.Rabbit.Publisher.Department.Accounting.Models.WorkerModel() { Id = worker.Id },
                    IBAN = worker.BankAccounts.FirstOrDefault().IBAN
                });

            #endregion

            #region İdari işler departmanının kendi envanterlerini ataması için rabbit e kayıt ekle

            if (worker.AAInventories == null)
                worker.AAInventories = new List<InventoryModel>();

            if (!worker.AAInventories.Any())
            {
                ServiceResultModel<List<InventoryModel>> defaultInventoriesServiceResult =
                    await _serviceCommunicator.Call<List<InventoryModel>>(
                        serviceName: _routeNameProvider.AA_GetInventoriesForNewWorker,
                        postData: null,
                        queryParameters: null,
                        headers: new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("TransactionIdentity", TransactionIdentity)
                        },
                        cancellationTokenSource);

                if (defaultInventoriesServiceResult.IsSuccess)
                {
                    worker.AAInventories.AddRange(defaultInventoriesServiceResult.Data);
                }
                else
                {
                    throw new CallException(
                        message: defaultInventoriesServiceResult.ErrorModel.Description,
                        endpoint:
                        !string.IsNullOrEmpty(defaultInventoriesServiceResult.SourceApiService)
                        ?
                        defaultInventoriesServiceResult.SourceApiService
                        :
                        $"{ApiServiceName}).{nameof(PersonService)}.{nameof(CreateWorkerAsync)}",
                        error: defaultInventoriesServiceResult.ErrorModel,
                        validation: defaultInventoriesServiceResult.Validation);
                }
            }

            _AAassignInventoryToWorkerPublisher.AddToBuffer(new Infrastructure.Communication.Mq.Rabbit.Publisher.Department.AA.Models.WorkerModel()
            {
                Id = worker.Id,
                Inventories = worker.AAInventories.Select(x => new Infrastructure.Communication.Mq.Rabbit.Publisher.Department.AA.Models.InventoryModel()
                {
                    FromDate = x.FromDate,
                    Id = x.Id,
                    ToDate = x.ToDate
                }).ToList()
            });

            #endregion

            #region IT departmanının kendi envanterlerini ataması için rabbit e kayıt ekle

            if (worker.ITInventories == null)
                worker.ITInventories = new List<InventoryModel>();

            if (!worker.ITInventories.Any())
            {
                ServiceResultModel<List<InventoryModel>> defaultInventoriesServiceResult =
                    await _serviceCommunicator.Call<List<InventoryModel>>(
                        serviceName: _routeNameProvider.IT_GetInventoriesForNewWorker,
                        postData: null,
                        queryParameters: null,
                        headers: new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("TransactionIdentity", TransactionIdentity)
                        },
                        cancellationTokenSource);

                if (defaultInventoriesServiceResult.IsSuccess)
                {
                    worker.ITInventories.AddRange(defaultInventoriesServiceResult.Data);
                }
                else
                {
                    throw new CallException(
                        message: defaultInventoriesServiceResult.ErrorModel.Description,
                        endpoint:
                        !string.IsNullOrEmpty(defaultInventoriesServiceResult.SourceApiService)
                        ?
                        defaultInventoriesServiceResult.SourceApiService
                        :
                        $"{ApiServiceName}).{nameof(PersonService)}.{nameof(CreateWorkerAsync)}",
                        error: defaultInventoriesServiceResult.ErrorModel,
                        validation: defaultInventoriesServiceResult.Validation);
                }
            }

            _ITAssignInventoryToWorkerPublisher.AddToBuffer(new Infrastructure.Communication.Mq.Rabbit.Publisher.Department.IT.Models.WorkerModel()
            {
                Id = worker.Id,
                Inventories = worker.ITInventories.Select(x => new Infrastructure.Communication.Mq.Rabbit.Publisher.Department.IT.Models.InventoryModel()
                {
                    FromDate = x.FromDate,
                    Id = x.Id,
                    ToDate = x.ToDate
                }).ToList()
            });

            #endregion

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            Task.WaitAll(new Task[]
            {
                _createBankAccountPublisher.PublishBufferAsync(cancellationTokenSource),
                _AAassignInventoryToWorkerPublisher.PublishBufferAsync(cancellationTokenSource),
                _ITAssignInventoryToWorkerPublisher.PublishBufferAsync(cancellationTokenSource)

            }, cancellationTokenSource.Token);

            if (_redisCacheDataProvider.TryGetValue(CACHED_WORKERS_KEY, out List<WorkerModel> cachedWorkers) && cachedWorkers != null)
            {
                cachedWorkers.Add(worker);

                _redisCacheDataProvider.Set(CACHED_WORKERS_KEY, cachedWorkers);
            }

            return worker.Id;
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
                    case DepartmentRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _departmentRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _departmentRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _departmentRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else
                            throw new Exception(
                                await _translationProvider.TranslateAsync("Tanimsiz.Geri.Alma", Region, cancellationToken: cancellationTokenSource.Token));
                        break;
                    case PersonRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _personRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _personRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _personRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else
                            throw new Exception(
                                await _translationProvider.TranslateAsync("Tanimsiz.Geri.Alma", Region, cancellationToken: cancellationTokenSource.Token));
                        break;
                    case TitleRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _titleRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _titleRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _titleRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else
                            throw new Exception(
                                await _translationProvider.TranslateAsync("Tanimsiz.Geri.Alma", Region, cancellationToken: cancellationTokenSource.Token));
                        break;
                    case WorkerRelationRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _workerRelationRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _workerRelationRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _workerRelationRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else
                            throw new Exception(
                                await _translationProvider.TranslateAsync("Tanimsiz.Geri.Alma", Region, cancellationToken: cancellationTokenSource.Token));
                        break;
                    case WorkerRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _workerRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _workerRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _workerRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
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
            _personRepository.Dispose();
            _departmentRepository.Dispose();
            _titleRepository.Dispose();
            _transactionItemRepository.Dispose();
            _transactionRepository.Dispose();
            _workerRelationRepository.Dispose();
            _workerRepository.Dispose();
            _unitOfWork.Dispose();
            _translationProvider.Dispose();
        }
    }
}
