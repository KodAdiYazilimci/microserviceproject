using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Buying.Services;
using Services.Api.Business.Departments.Buying.Util.Validation.Request.CreateInventoryRequest;
using Services.Api.Business.Departments.Buying.Util.Validation.Request.ValidateCostInventory;
using Services.Communication.Http.Broker.Department.Buying.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Controllers
{
    [Route("Request")]
    public class RequestController : BaseController
    {
        private readonly RequestService _requestService;

        public RequestController(RequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        [Route(nameof(GetInventoryRequests))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetInventoryRequests(CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<InventoryRequestModel>>(async () =>
            {
                return await _requestService.GetInventoryRequestsAsync(cancellationTokenSource);
            },
            services: _requestService);
        }

        [HttpPost]
        [Route(nameof(CreateInventoryRequest))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateInventoryRequest([FromBody] InventoryRequestModel requestModel, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateInventoryRequestValidator.ValidateAsync(requestModel, cancellationTokenSource);

                return await _requestService.CreateInventoryRequestAsync(requestModel, cancellationTokenSource);
            },
            services: _requestService);
        }

        [HttpPost]
        [Route(nameof(ValidateCostInventory))]
        [Authorize(Roles = "ApiUser,QueueUser")]
        public async Task<IActionResult> ValidateCostInventory([FromBody] DecidedCostModel decidedCost, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await ValidateCostInventoryValidator.ValidateAsync(decidedCost, cancellationTokenSource);

                return await _requestService.ValidateCostInventoryAsync(decidedCost, cancellationTokenSource);
            },
            services: _requestService);
        }
    }
}
