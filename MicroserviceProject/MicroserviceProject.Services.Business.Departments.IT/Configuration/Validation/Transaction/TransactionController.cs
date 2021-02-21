using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors;
using MicroserviceProject.Services.Business.Departments.IT.Services;
using MicroserviceProject.Services.Business.Departments.IT.Util.Validation.Transaction;
using MicroserviceProject.Services.Transaction.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.IT.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : Controller
    {
        private readonly InventoryService _inventoryService;

        public TransactionController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;

            if (Request.Headers.TryGetValue("TransactionIdentity", out StringValues value))
            {
                _inventoryService.TransactionIdentity = value;
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

                if (rollbackModel.Modules.Contains(InventoryService.MODULE_NAME))
                {
                    rollbackResult = await _inventoryService.RollbackTransactionAsync(rollbackModel, cancellationToken);
                }

                return Ok(new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = rollbackResult,
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity = _inventoryService.TransactionIdentity,
                        Modules = new List<string>()
                        {
                            InventoryService.MODULE_NAME
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
                        TransactionIdentity = _inventoryService.TransactionIdentity,
                        Modules = new List<string>()
                        {
                            InventoryService.MODULE_NAME
                        }
                    }
                });
            }
        }
    }
}