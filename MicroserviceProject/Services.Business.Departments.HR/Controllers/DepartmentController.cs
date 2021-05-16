using Infrastructure.Transaction.ExecutionHandler;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Business.Departments.HR.Models;
using Services.Business.Departments.HR.Services;
using Services.Business.Departments.HR.Util.Validation.Department.CreateDepartment;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.HR.Controllers
{
    [Authorize]
    [Route("Department")]
    public class DepartmentController : BaseController
    {
        private readonly DepartmentService _departmentService;

        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Route(nameof(GetDepartments))]
        public async Task<IActionResult> GetDepartments(CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_departmentService);

            return await ServiceExecuter.ExecuteServiceAsync<List<DepartmentModel>>(async () =>
            {
                return await _departmentService.GetDepartmentsAsync(cancellationTokenSource);
            },
            services: _departmentService);
        }

        [HttpPost]
        [Route(nameof(CreateDepartment))]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentModel department, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_departmentService);

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await CreateDepartmentValidator.ValidateAsync(department, cancellationTokenSource);

                return await _departmentService.CreateDepartmentAsync(department, cancellationTokenSource);
            },
            services: _departmentService);
        }
    }
}
