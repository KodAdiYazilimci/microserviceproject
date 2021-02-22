using MicroserviceProject.Services.Business.Departments.IT.Services;
using MicroserviceProject.Services.Business.Departments.IT.Util.Validation.Transaction;
using MicroserviceProject.Services.Transaction.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationToken cancellationToken)
        {
            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await RollbackTransactionValidator.ValidateAsync(rollbackModel, cancellationToken);

                int rollbackResult = 0;

                if (rollbackModel.Modules.Contains(_inventoryService.ServiceName))
                {
                    rollbackResult = await _inventoryService.RollbackTransactionAsync(rollbackModel, cancellationToken);
                }

                return rollbackResult;
            },
            services: _inventoryService);
        }
    }
}