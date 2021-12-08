using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Transaction.Recovery;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Buying.Services;
using Services.Api.Business.Departments.Buying.Util.Validation.Transaction;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Controllers
{
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
        [Authorize(Roles = "ApiUser")]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
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
