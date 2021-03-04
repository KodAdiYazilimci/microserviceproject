using MicroserviceProject.Services.Business.Departments.Finance.Services;
using MicroserviceProject.Services.Business.Departments.Finance.Util.Validation.Cost.CreateCost;
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
        public async Task<IActionResult> GetDecidedCosts(CancellationToken cancellationToken)
        {
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _costService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<List<DecidedCostModel>>(async () =>
            {
                return await _costService.GetDecidedCostsAsync(cancellationToken);
            },
            services: _costService);
        }

        [HttpPost]
        [Route(nameof(CreateCost))]
        public async Task<IActionResult> CreateCost([FromBody] DecidedCostModel cost, CancellationToken cancellationToken)
        {
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _costService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await CreateCostValidator.ValidateAsync(cost, cancellationToken);

                return await _costService.CreateDecidedCostAsync(cost, cancellationToken);
            },
            services: _costService);
        }
    }
}
