using MicroserviceProject.Services.Business.Departments.AA.Services;
using MicroserviceProject.Services.Business.Departments.AA.Util.Validation.Transaction;
using MicroserviceProject.Services.Transaction.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.AA.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : Controller
    {
        private readonly InventoryService _inventoryService;

        public TransactionController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationTokenSource cancellationTokenSource)
        {
            if (Request != null && Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _inventoryService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await RollbackTransactionValidator.ValidateAsync(rollbackModel, cancellationTokenSource);

                int rollbackResult = 0;

                if (rollbackModel.Modules.Contains(_inventoryService.ServiceName))
                {
                    rollbackResult = await _inventoryService.RollbackTransactionAsync(rollbackModel, cancellationTokenSource);
                }

                return rollbackResult;
            },
            services: _inventoryService);
        }
    }
}