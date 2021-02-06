using MicroserviceProject.Services.Business.Departments.HR.Repositories;
using MicroserviceProject.Services.Business.Departments.Model.Department.HR;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Services
{
    public class DepartmentService
    {
        private readonly DepartmentRepository _departmentRepository;

        public DepartmentService(DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<List<DepartmentModel>> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            return await _departmentRepository.GetDepartmentsAsync(cancellationToken);
        }

        public async Task CreateDepartmentAsync(DepartmentModel department, CancellationToken cancellationToken)
        {
            await _departmentRepository.CreateDepartmentAsync(department, cancellationToken);
        }
    }
}
