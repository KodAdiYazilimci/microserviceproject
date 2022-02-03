using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Transaction.Recovery;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Business.Departments.Finance.Services;
using Services.Business.Departments.Finance.Util.Validation.Transaction;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Controllers
{
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
        [Authorize(Roles = "ApiUser")]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await RollbackTransactionValidator.ValidateAsync(rollbackModel, cancellationTokenSource);

                int rollbackResult = 0;

                if (rollbackModel.Modules.Contains(_costService.ServiceName))
                {
                    rollbackResult = await _costService.GetProductionRequestsAsync(rollbackModel, cancellationTokenSource);
                }

                return rollbackResult;
            },
            services: _costService);
        }
    }
}