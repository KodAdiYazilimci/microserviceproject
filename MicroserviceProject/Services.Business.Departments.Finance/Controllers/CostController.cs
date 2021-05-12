using MicroserviceProject.Infrastructure.Transaction.ExecutionHandler;
using MicroserviceProject.Services.Business.Departments.Finance.Services;
using MicroserviceProject.Services.Business.Departments.Finance.Util.Validation.Cost.CreateCost;
using MicroserviceProject.Services.Business.Departments.Finance.Util.Validation.Cost.DecideCost;
using MicroserviceProject.Services.Model.Department.Finance;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Finance.Controllers
{
    [Authorize]
    [Route("Cost")]
    public class CostController : Controller
    {
        private readonly CostService _costService;

        public CostController(CostService costService)
        {
            _costService = costService;
        }

        [HttpGet]
        [Route(nameof(GetDecidedCosts))]
        public async Task<IActionResult> GetDecidedCosts(CancellationTokenSource cancellationTokenSource)
        {
            if (Request != null && Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _costService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<List<DecidedCostModel>>(async () =>
            {
                return await _costService.GetDecidedCostsAsync(cancellationTokenSource);
            },
            services: _costService);
        }

        [HttpPost]
        [Route(nameof(CreateCost))]
        public async Task<IActionResult> CreateCost([FromBody] DecidedCostModel cost, CancellationTokenSource cancellationTokenSource)
        {
            if (Request != null && Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _costService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await CreateCostValidator.ValidateAsync(cost, cancellationTokenSource);

                return await _costService.CreateDecidedCostAsync(cost, cancellationTokenSource);
            },
            services: _costService);
        }

        [HttpPost]
        [Route(nameof(DecideCost))]
        public async Task<IActionResult> DecideCost([FromBody] DecidedCostModel cost, CancellationTokenSource cancellationTokenSource)
        {
            if (Request != null && Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _costService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await DecideCostValidator.ValidateAsync(cost, cancellationTokenSource);

                if (cost.Approved)
                    return await _costService.ApproveCostAsync(cost.Id, cancellationTokenSource);
                else
                    return await _costService.RejectCostAsync(cost.Id, cancellationTokenSource);
            },
            services: _costService);
        }
    }
}
