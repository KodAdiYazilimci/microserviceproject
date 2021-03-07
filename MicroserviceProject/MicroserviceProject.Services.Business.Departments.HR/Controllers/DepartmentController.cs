using MicroserviceProject.Services.Business.Departments.HR.Services;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Department.CreateDepartment;
using MicroserviceProject.Services.Model.Department.HR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Controllers
{
    [Authorize]
    [Route("Department")]
    public class DepartmentController : Controller
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
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _departmentService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

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
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _departmentService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await CreateDepartmentValidator.ValidateAsync(department, cancellationTokenSource);

                return await _departmentService.CreateDepartmentAsync(department, cancellationTokenSource);
            },
            services: _departmentService);
        }
    }
}
