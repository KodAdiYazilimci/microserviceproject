using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Model.Department.HR;
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
    public class DepartmentService : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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
            DepartmentRepository departmentRepository)
        {
            _mapper = mapper;
            _cacheDataProvider = cacheDataProvider;
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Departmanların listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<DepartmentModel>> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            if (_cacheDataProvider.TryGetValue(CACHED_DEPARTMENTS_KEY, out List<DepartmentModel> cachedDepartments)
                &&
                cachedDepartments != null && cachedDepartments.Any())
            {
                return cachedDepartments;
            }

            List<DepartmentEntity> departments = await _departmentRepository.GetListAsync(cancellationToken);

            List<DepartmentModel> mappedDepartments =
                _mapper.Map<List<DepartmentEntity>, List<DepartmentModel>>(departments);

            _cacheDataProvider.Set(CACHED_DEPARTMENTS_KEY, mappedDepartments);

            return mappedDepartments;
        }

        /// <summary>
        /// Yeni departman oluşturur
        /// </summary>
        /// <param name="department">Oluşturulacak departman nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateDepartmentAsync(DepartmentModel department, CancellationToken cancellationToken)
        {
            DepartmentEntity mappedDepartment = _mapper.Map<DepartmentModel, DepartmentEntity>(department);

            int createdDepartmentId = await _departmentRepository.CreateAsync(mappedDepartment, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            department.Id = createdDepartmentId;

            if (_cacheDataProvider.TryGetValue(CACHED_DEPARTMENTS_KEY, out List<DepartmentModel> cachedDepartments))
            {
                cachedDepartments.Add(department);
                _cacheDataProvider.Set(CACHED_DEPARTMENTS_KEY, cachedDepartments);
            }
            else
            {
                List<DepartmentModel> departments = await GetDepartmentsAsync(cancellationToken);

                departments.Add(department);

                _cacheDataProvider.Set(CACHED_DEPARTMENTS_KEY, departments);
            }

            return createdDepartmentId;
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
                    _departmentRepository.Dispose();
                    _unitOfWork.Dispose();
                }

                disposed = true;
            }
        }
    }
}
