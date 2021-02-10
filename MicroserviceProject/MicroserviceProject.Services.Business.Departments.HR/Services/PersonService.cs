using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator;
using MicroserviceProject.Infrastructure.Communication.Moderator.Providers;
using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Business.Model.Department.Accounting;
using MicroserviceProject.Services.Business.Model.Department.HR;
using MicroserviceProject.Services.Business.Util.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Services
{
    /// <summary>
    /// Kişi işlemleri iş mantığı sınıfı
    /// </summary>
    public class PersonService : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Önbelleğe alınan kişilerin önbellekteki adı
        /// </summary>
        private const string CACHED_PEOPLE_KEY = "MicroserviceProject.Services.Business.Departments.HR.People";
        private const string CACHED_WORKERS_KEY = "MicroserviceProject.Services.Business.Departments.HR.Workers";
        private const string CACHED_TITLES_KEY = "MicroserviceProject.Services.Business.Departments.HR.Titles";

        /// <summary>
        /// Rediste tutulan önbellek yönetimini sağlayan sınıf
        /// </summary>
        private readonly CacheDataProvider _cacheDataProvider;

        /// <summary>
        /// Mapping işlemleri için mapper nesnesi
        /// </summary>
        private readonly IMapper _mapper;

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
        /// Kişi işlemleri iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="routeNameProvider">Servislerin rota isimlerini sağlayan sınıf</param>
        /// <param name="serviceCommunicator">Diğer servislerle iletişim kuracak ara bulucu</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="cacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="personRepository">Kişi tablosu için repository sınıfı</param>
        /// <param name="titleRepository">Kişi tablosu için repository sınıfı</param>
        /// <param name="workerRepository">Çalışan tablosu için repository sınıfı</param>
        /// <param name="workerRelationRepository">Çalışan ilişkileri tablosu için repository sınıfı</param>
        public PersonService(
            IMapper mapper,
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator,
            IUnitOfWork unitOfWork,
            CacheDataProvider cacheDataProvider,
            PersonRepository personRepository,
            TitleRepository titleRepository,
            WorkerRepository workerRepository,
            WorkerRelationRepository workerRelationRepository)
        {
            _cacheDataProvider = cacheDataProvider;
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;

            _personRepository = personRepository;
            _titleRepository = titleRepository;
            _workerRepository = workerRepository;
            _workerRelationRepository = workerRelationRepository;
        }

        /// <summary>
        /// Kişilerin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<PersonModel>> GetPeopleAsync(CancellationToken cancellationToken)
        {
            if (_cacheDataProvider.TryGetValue(CACHED_PEOPLE_KEY, out List<PersonModel> cachedPeople)
                &&
                cachedPeople != null && cachedPeople.Any())
            {
                return cachedPeople;
            }

            List<PersonEntity> people = await _personRepository.GetListAsync(cancellationToken);

            List<PersonModel> mappedPeople =
                _mapper.Map<List<PersonEntity>, List<PersonModel>>(people);

            _cacheDataProvider.Set(CACHED_PEOPLE_KEY, mappedPeople);

            return mappedPeople;
        }

        /// <summary>
        /// Yeni kişi oluşturur
        /// </summary>
        /// <param name="person">Oluşturulacak kişi nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreatePersonAsync(PersonModel person, CancellationToken cancellationToken)
        {
            PersonEntity mappedPerson = _mapper.Map<PersonModel, PersonEntity>(person);

            int createdPersonId = await _personRepository.CreateAsync(mappedPerson, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            person.Id = createdPersonId;

            if (_cacheDataProvider.TryGetValue(CACHED_PEOPLE_KEY, out List<PersonModel> cachedPeople))
            {
                cachedPeople.Add(person);
                _cacheDataProvider.Set(CACHED_PEOPLE_KEY, cachedPeople);
            }
            else
            {
                List<PersonModel> people = await GetPeopleAsync(cancellationToken);

                people.Add(person);

                _cacheDataProvider.Set(CACHED_PEOPLE_KEY, people);
            }

            return createdPersonId;
        }

        /// <summary>
        /// Ünvanların listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<TitleModel>> GetTitlesAsync(CancellationToken cancellationToken)
        {
            if (_cacheDataProvider.TryGetValue(CACHED_TITLES_KEY, out List<TitleModel> cachedTitles)
                &&
                cachedTitles != null && cachedTitles.Any())
            {
                return cachedTitles;
            }

            List<TitleEntity> titles = await _titleRepository.GetListAsync(cancellationToken);

            List<TitleModel> mappedTitles =
                _mapper.Map<List<TitleEntity>, List<TitleModel>>(titles);

            _cacheDataProvider.Set(CACHED_TITLES_KEY, mappedTitles);

            return mappedTitles;
        }

        /// <summary>
        /// Yeni ünvan oluşturur
        /// </summary>
        /// <param name="title">Oluşturulacak ünvan nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateTitleAsync(TitleModel title, CancellationToken cancellationToken)
        {
            TitleEntity mappedTitles = _mapper.Map<TitleModel, TitleEntity>(title);

            int createdTitleId = await _titleRepository.CreateAsync(mappedTitles, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            title.Id = createdTitleId;

            if (_cacheDataProvider.TryGetValue(CACHED_TITLES_KEY, out List<TitleModel> cachedTitles))
            {
                cachedTitles.Add(title);
                _cacheDataProvider.Set(CACHED_TITLES_KEY, cachedTitles);
            }
            else
            {
                List<TitleModel> titles = await GetTitlesAsync(cancellationToken);

                titles.Add(title);

                _cacheDataProvider.Set(CACHED_TITLES_KEY, titles);
            }

            return createdTitleId;
        }

        /// <summary>
        /// Çalışanların listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<WorkerModel>> GetWorkersAsync(CancellationToken cancellationToken)
        {
            if (_cacheDataProvider.TryGetValue(CACHED_WORKERS_KEY, out List<WorkerModel> cachedWorkers)
                &&
                cachedWorkers != null && cachedWorkers.Any())
            {
                return cachedWorkers;
            }

            List<WorkerEntity> workers = await _workerRepository.GetListAsync(cancellationToken);

            List<WorkerModel> mappedWorkers =
                _mapper.Map<List<WorkerEntity>, List<WorkerModel>>(workers);

            _cacheDataProvider.Set(CACHED_WORKERS_KEY, mappedWorkers);

            return mappedWorkers;
        }

        /// <summary>
        /// Yeni çalışan oluşturur
        /// </summary>
        /// <param name="worker">Oluşturulacak çalışan nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateWorkerAsync(WorkerModel worker, CancellationToken cancellationToken)
        {
            WorkerEntity mappedWorker = _mapper.Map<WorkerModel, WorkerEntity>(worker);

            worker.Id = await _workerRepository.CreateAsync(mappedWorker, cancellationToken);

            ServiceResult<int> createBankAccountServiceResult = await _serviceCommunicator.Call<int>(
                 serviceName: _routeNameProvider.Accounting_CreateBankAccount,
                 postData: new BankAccountModel()
                 {
                     Worker = worker,
                     IBAN = worker.BankAccounts.FirstOrDefault().IBAN
                 },
                 queryParameters: null,
                 cancellationToken: cancellationToken);

            if (!createBankAccountServiceResult.IsSuccess)
            {
                throw new Exception(createBankAccountServiceResult.Error.Description);
            }

            await _unitOfWork.SaveAsync(cancellationToken);


            if (_cacheDataProvider.TryGetValue(CACHED_WORKERS_KEY, out List<WorkerModel> cachedWorkers))
            {
                cachedWorkers.Add(worker);
                _cacheDataProvider.Set(CACHED_WORKERS_KEY, cachedWorkers);
            }
            else
            {
                List<WorkerModel> workers = await GetWorkersAsync(cancellationToken);

                workers.Add(worker);

                _cacheDataProvider.Set(CACHED_WORKERS_KEY, workers);
            }

            return worker.Id;
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
                    _personRepository.Dispose();
                    _unitOfWork.Dispose();
                }

                disposed = true;
            }
        }
    }
}
