using AutoMapper;

using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;
using MicroserviceProject.Services.Business.Departments.Model.Department.HR;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Services
{
    /// <summary>
    /// Departman işlemleri iş mantığı sınıfı
    /// </summary>
    public class DepartmentService
    {
        /// <summary>
        /// Mapping işlemleri için mapper nesnesi
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Departman tablosu için repository sınıfı
        /// </summary>
        private readonly DepartmentRepository _departmentRepository;

        /// <summary>
        /// Departman işlemleri iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="departmentRepository">Departman tablosu için repository sınıfı</param>
        public DepartmentService(
            IMapper mapper,
            DepartmentRepository departmentRepository)
        {
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        /// <summary>
        /// Departmanların listesini verir
        /// </summary>
        /// <param name="cancellationToken">ptal tokenı</param>
        /// <returns></returns>
        public async Task<List<DepartmentModel>> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            List<DepartmentEntity> departments = await _departmentRepository.GetDepartmentsAsync(cancellationToken);

            List<DepartmentModel> mappedDepartments =
                _mapper.Map<List<DepartmentEntity>, List<DepartmentModel>>(departments);

            return mappedDepartments;
        }

        /// <summary>
        /// Yeni departman oluşturur
        /// </summary>
        /// <param name="department">Oluşturulacak departman nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task CreateDepartmentAsync(DepartmentModel department, CancellationToken cancellationToken)
        {
            DepartmentEntity mappedDepartment = _mapper.Map<DepartmentModel, DepartmentEntity>(department);

            await _departmentRepository.CreateDepartmentAsync(mappedDepartment, cancellationToken);
        }
    }
}
