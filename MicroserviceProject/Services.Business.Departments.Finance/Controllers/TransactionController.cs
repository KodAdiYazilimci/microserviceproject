using Infrastructure.Transaction.ExecutionHandler;
using Infrastructure.Transaction.Recovery;
using Services.Business.Departments.Finance.Services;
using Services.Business.Departments.Finance.Util.Validation.Transaction;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : BaseController
    {
        private readonly CostService _costService;

        public TransactionController(CostService inventoryService)
        {
            _costService = inventoryService;
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_costService);

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await RollbackTransactionValidator.ValidateAsync(rollbackModel, cancellationTokenSource);

                int rollbackResult = 0;

                if (rollbackModel.Modules.Contains(_costService.ServiceName))
                {
                    rollbackResult = await _costService.RollbackTransactionAsync(rollbackModel, cancellationTokenSource);
                }

                return rollbackResult;
            },
            services: _costService);
        }
    }
}