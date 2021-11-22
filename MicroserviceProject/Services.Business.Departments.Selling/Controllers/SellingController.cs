using Services.Communication.Http.Broker.Department.Selling.Models;

using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Business.Departments.Selling.Services;
using Services.Business.Departments.Selling.Util.Validation.Selling;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Selling.Controllers
{
    [Authorize]
    [Route("Selling")]
    public class SellingController : BaseController
    {
        private readonly SellingService _sellingService;

        public SellingController(SellingService sellingService)
        {
            _sellingService = sellingService;
        }

        [HttpGet]
        [Route(nameof(GetSolds))]
        public async Task<IActionResult> GetSolds(CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<SellModel>>(async () =>
            {
                return await _sellingService.GetSoldsAsync(cancellationTokenSource);
            },
            services: _sellingService);
        }

        [HttpPost]
        [Route(nameof(CreateSelling))]
        public async Task<IActionResult> CreateSelling([FromBody] SellModel sellModel, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateSellingValidator.ValidateAsync(sellModel, cancellationTokenSource);

                return await _sellingService.CreateSellingAsync(sellModel, cancellationTokenSource);
            },
            services: _sellingService);
        }

        [HttpPost]
        [Route(nameof(NotifyProductionRequest))]
        public async Task<IActionResult> NotifyProductionRequest([FromBody] ProductionRequestModel productionRequest, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await NotifyProductionRequestValidator.ValidateAsync(productionRequest, cancellationTokenSource);

                return await _sellingService.NotifyProductionRequestAsync(productionRequest, cancellationTokenSource);
            },
            services: _sellingService);
        }
    }
}
