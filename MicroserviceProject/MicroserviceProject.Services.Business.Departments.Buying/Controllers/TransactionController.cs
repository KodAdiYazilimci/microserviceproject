using MicroserviceProject.Services.Business.Departments.Buying.Services;
using MicroserviceProject.Services.Business.Departments.Buying.Util.Validation.Transaction;
using MicroserviceProject.Services.Transaction.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Buying.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : Controller
    {
        private readonly RequestService _inventoryService;

        public TransactionController(RequestService inventoryService)
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