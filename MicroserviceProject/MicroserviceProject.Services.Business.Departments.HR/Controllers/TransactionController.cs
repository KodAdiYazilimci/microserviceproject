using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors;
using MicroserviceProject.Services.Business.Departments.HR.Services;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Transaction;
using MicroserviceProject.Services.Transaction.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : Controller
    {
        private readonly DepartmentService _departmentService;
        private readonly PersonService _personService;

        public TransactionController(
            DepartmentService departmentService,
            PersonService personService)
        {
            _departmentService = departmentService;
            _personService = personService;

            if (Request.Headers.TryGetValue("TransactionIdentity", out StringValues value))
            {
                _departmentService.TransactionIdentity = value;
                _personService.TransactionIdentity = value;
            }
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult =
                     await RollbackTransactionValidator.ValidateAsync(rollbackModel, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                int rollbackResult = 0;

                if (rollbackModel.Modules.Contains(DepartmentService.MODULE_NAME))
                {
                    rollbackResult = await _departmentService.RollbackTransactionAsync(rollbackModel, cancellationToken);
                }

                if (rollbackModel.Modules.Contains(PersonService.MODULE_NAME))
                {
                    rollbackResult = await _departmentService.RollbackTransactionAsync(rollbackModel, cancellationToken);
                }

                return Ok(new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = rollbackResult,
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity = _departmentService.TransactionIdentity,
                        Modules = new List<string>()
                        {
                            DepartmentService.MODULE_NAME,
                            PersonService.MODULE_NAME
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() },
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity = _departmentService.TransactionIdentity,
                        Modules = new List<string>()
                        {
                            DepartmentService.MODULE_NAME,
                            PersonService.MODULE_NAME
                        }
                    }
                });
            }
        }
    }
}