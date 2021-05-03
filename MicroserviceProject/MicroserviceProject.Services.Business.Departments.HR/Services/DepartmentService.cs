using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Disposing;
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

namespace MicroserviceProject.Services.Business.Departments.HR.Services
{
    /// <summary>
    /// Departman işlemleri iş mantığı sınıfı
    /// </summary>
    public class DepartmentService : BaseService, IRollbackableAsync<int>, IDisposable, IDisposableInjections
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "MicroserviceProject.Services.Business.Departments.HR.Services.DepartmentService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "MicroserviceProject.Services.Business.Departments.HR";

        /// <summary>
        /// Önbelleğe alınan departmanların önbellekteki adı
        /// </summary>
        private const string CACHED_DEPARTMENTS_KEY = "MicroserviceProject.Services.Business.Departments.HR.Departments";

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
        /// Departman tablosu için repository sınıfı
        /// </summary>
        private readonly DepartmentRepository _departmentRepository;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Departman işlemleri iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="cacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf</param>
        /// <param name="departmentRepository">Departman tablosu için repository sınıfı</param>
        public DepartmentService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            CacheDataProvider cacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            DepartmentRepository departmentRepository)
        {
            _mapper = mapper;
            _cacheDataProvider = cacheDataProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;

            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Departmanların listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<DepartmentModel>> GetDepartmentsAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_cacheDataProvider.TryGetValue(CACHED_DEPARTMENTS_KEY, out List<DepartmentModel> cachedDepartments)
                &&
                cachedDepartments != null && cachedDepartments.Any())
            {
                return cachedDepartments;
            }

            List<DepartmentEntity> departments = await _departmentRepository.GetListAsync(cancellationTokenSource);

            List<DepartmentModel> mappedDepartments =
                _mapper.Map<List<DepartmentEntity>, List<DepartmentModel>>(departments);

            _cacheDataProvider.Set(CACHED_DEPARTMENTS_KEY, mappedDepartments);

            return mappedDepartments;
        }

        /// <summary>
        /// Yeni departman oluşturur
        /// </summary>
        /// <param name="department">Oluşturulacak departman nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateDepartmentAsync(DepartmentModel department, CancellationTokenSource cancellationTokenSource)
        {
            DepartmentEntity mappedDepartment = _mapper.Map<DepartmentModel, DepartmentEntity>(department);

            int createdDepartmentId = await _departmentRepository.CreateAsync(mappedDepartment, cancellationTokenSource);

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
                            Identity = createdDepartmentId,
                            DataSet = DepartmentRepository.TABLE_NAME,
                            RollbackType= RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            department.Id = createdDepartmentId;

            if (_cacheDataProvider.TryGetValue(CACHED_DEPARTMENTS_KEY, out List<DepartmentModel> cachedDepartments) && cachedDepartments != null)
            {
                cachedDepartments.Add(department);

                _cacheDataProvider.Set(CACHED_DEPARTMENTS_KEY, cachedDepartments);
            }

            return createdDepartmentId;
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
                            throw new Exception("Tanımlanmamış geri alma biçimi");
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
            _cacheDataProvider.Dispose();
            _departmentRepository.Dispose();
            _transactionItemRepository.Dispose();
            _transactionRepository.Dispose();
            _unitOfWork.Dispose();
        }
    }
}
