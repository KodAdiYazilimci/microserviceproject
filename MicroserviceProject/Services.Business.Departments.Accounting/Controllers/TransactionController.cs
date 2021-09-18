using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Transaction.Recovery;
using Services.Business.Departments.Accounting.Services;
using Services.Business.Departments.Accounting.Util.Validation.Transaction;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Accounting.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : BaseController
    {
        private readonly BankService _bankService;

        public TransactionController(BankService bankService)
        {
            _bankService = bankService;
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await RollbackTransactionValidator.ValidateAsync(rollbackModel, cancellationTokenSource);

                int rollbackResult = 0;

                if (rollbackModel.Modules.Contains(_bankService.ServiceName))
                {
                    rollbackResult = await _bankService.RollbackTransactionAsync(rollbackModel, cancellationTokenSource);
                }

                return rollbackResult;
            },
            services: _bankService);
        }
    }
}