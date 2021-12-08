using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Selling.Services;
using Services.Api.Business.Departments.Selling.Util.Validation.Selling;
using Services.Communication.Http.Broker.Department.Selling.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Selling.Controllers
{
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
        [Authorize(Roles = "ApiUser,GatewayUser")]
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
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
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
        [Authorize(Roles = "ApiUser,QueueUser")]
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
