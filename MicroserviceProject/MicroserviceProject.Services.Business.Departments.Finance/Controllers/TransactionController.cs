using MicroserviceProject.Services.Business.Departments.Finance.Services;
using MicroserviceProject.Services.Business.Departments.Finance.Util.Validation.Transaction;
using MicroserviceProject.Services.Transaction.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Finance.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : Controller
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
            if (Request != null && Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _costService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

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