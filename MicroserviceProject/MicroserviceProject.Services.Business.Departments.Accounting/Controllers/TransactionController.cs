using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors;
using MicroserviceProject.Services.Business.Departments.Accounting.Services;
using MicroserviceProject.Services.Business.Departments.Accounting.Util.Validation.Transaction;
using MicroserviceProject.Services.Transaction.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Accounting.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : Controller
    {
        private readonly BankService _bankService;

        public TransactionController(BankService bankService)
        {
            _bankService = bankService;

            if (Request.Headers.TryGetValue("TransactionIdentity", out StringValues value))
            {
                _bankService.TransactionIdentity = value;
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

                if (rollbackModel.Modules.Contains(BankService.MODULE_NAME))
                {
                    rollbackResult = await _bankService.RollbackTransactionAsync(rollbackModel, cancellationToken);
                }

                return Ok(new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = rollbackResult,
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity = _bankService.TransactionIdentity,
                        Modules = new List<string>()
                        {
                            BankService.MODULE_NAME
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
                        TransactionIdentity = _bankService.TransactionIdentity,
                        Modules = new List<string>()
                        {
                            BankService.MODULE_NAME
                        }
                    }
                });
            }
        }
    }
}