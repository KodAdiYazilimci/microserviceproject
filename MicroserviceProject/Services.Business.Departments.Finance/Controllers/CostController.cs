using Infrastructure.Communication.Http.Wrapper;
using Services.Business.Departments.Finance.Services;
using Services.Business.Departments.Finance.Util.Validation.Cost.CreateCost;
using Services.Business.Departments.Finance.Util.Validation.Cost.DecideCost;
using Services.Model.Department.Finance;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Controllers
{
    [Authorize]
    [Route("Cost")]
    public class CostController : BaseController
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
            SetServiceDefaults(_costService);

            return await HttpResponseWrapper.WrapAsync<List<DecidedCostModel>>(async () =>
            {
                return await _costService.GetDecidedCostsAsync(cancellationTokenSource);
            },
            services: _costService);
        }

        [HttpPost]
        [Route(nameof(CreateCost))]
        public async Task<IActionResult> CreateCost([FromBody] DecidedCostModel cost, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_costService);

            return await HttpResponseWrapper.WrapAsync<int>(async () =>
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
            SetServiceDefaults(_costService);

            return await HttpResponseWrapper.WrapAsync<int>(async () =>
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
