﻿
using Infrastructure.Communication.Model.Department.Buying;
using Infrastructure.Transaction.ExecutionHandler;
using Services.Business.Departments.Buying.Services;
using Services.Business.Departments.Buying.Util.Validation.Request.CreateInventoryRequest;
using Services.Business.Departments.Buying.Util.Validation.Request.ValidateCostInventory;
using Services.Model.Department.Finance;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Buying.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetInventoryRequests(CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_requestService);

            return await ServiceExecuter.ExecuteServiceAsync<List<InventoryRequestModel>>(async () =>
            {
                return await _requestService.GetInventoryRequestsAsync(cancellationTokenSource);
            },
            services: _requestService);
        }

        [HttpPost]
        [Route(nameof(CreateInventoryRequest))]
        public async Task<IActionResult> CreateInventoryRequest([FromBody] InventoryRequestModel requestModel, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_requestService);

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await CreateInventoryRequestValidator.ValidateAsync(requestModel, cancellationTokenSource);

                return await _requestService.CreateInventoryRequestAsync(requestModel, cancellationTokenSource);
            },
            services: _requestService);
        }

        [HttpPost]
        [Route(nameof(ValidateCostInventory))]
        public async Task<IActionResult> ValidateCostInventory([FromBody] DecidedCostModel decidedCost, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_requestService);

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await ValidateCostInventoryValidator.ValidateAsync(decidedCost, cancellationTokenSource);

                return await _requestService.ValidateCostInventoryAsync(decidedCost, cancellationTokenSource);
            },
            services: _requestService);
        }
    }
}