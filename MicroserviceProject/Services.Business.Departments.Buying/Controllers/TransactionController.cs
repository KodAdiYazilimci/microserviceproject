using Infrastructure.Transaction.ExecutionHandler;
using Infrastructure.Transaction.Recovery;
using Services.Business.Departments.Buying.Services;
using Services.Business.Departments.Buying.Util.Validation.Transaction;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Buying.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : BaseController
    {
        private readonly RequestService _inventoryService;

        public TransactionController(RequestService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_inventoryService);

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