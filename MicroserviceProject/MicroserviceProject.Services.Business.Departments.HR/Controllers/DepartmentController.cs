using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors;
using MicroserviceProject.Services.Business.Departments.HR.Services;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Department.CreateDepartment;
using MicroserviceProject.Services.Model.Department.HR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
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
        public async Task<IActionResult> GetDepartments(CancellationToken cancellationToken)
        {
            try
            {
                List<DepartmentModel> departments =
                    await _departmentService.GetDepartmentsAsync(cancellationToken);

                ServiceResult<List<DepartmentModel>> serviceResult = new ServiceResult<List<DepartmentModel>>()
                {
                    IsSuccess = true,
                    Data = departments
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpPost]
        [Route(nameof(CreateDepartment))]
        public async Task<IActionResult> CreateDepartment(
            [FromBody] DepartmentModel department,
            CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult =
                    await CreateDepartmentValidator.ValidateAsync(department, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                int generatedId = await _departmentService.CreateDepartmentAsync(department, cancellationToken);

                ServiceResult<int> serviceResult = new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = generatedId
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }
    }
}
