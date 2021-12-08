﻿using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Business.Departments.Finance.Services;
using Services.Business.Departments.Finance.Util.Validation.Request.CreateProductionRequest;
using Services.Communication.Http.Broker.Department.Finance.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Controllers
{
    [Route("ProductionRequest")]
    public class ProductionRequestController : BaseController
    {
        private readonly ProductionRequestService _productionRequestService;

        public ProductionRequestController(ProductionRequestService productionRequestService)
        {
            _productionRequestService = productionRequestService;
        }

        [HttpGet]
        [Route(nameof(GetProductionRequests))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetProductionRequests(CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<ProductionRequestModel>>(async () =>
            {
                return await _productionRequestService.GetProductionRequestsAsync(cancellationTokenSource);
            },
            services: _productionRequestService);
        }

        [Route(nameof(CreateProductionRequest))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateProductionRequest([FromBody] ProductionRequestModel productionRequest, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateProductionRequestValidator.ValidateAsync(productionRequest, cancellationTokenSource);

                return await _productionRequestService.CreateProductionRequestAsync(productionRequest, cancellationTokenSource);
            },
            services: _productionRequestService);
        }


        [HttpPost]
        [Route(nameof(DecideProductionRequest))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> DecideProductionRequest([FromBody] ProductionRequestModel productionRequest, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await DecideProductionRequestValidator.ValidateAsync(productionRequest, cancellationTokenSource);

                if (productionRequest.Approved)
                    return await _productionRequestService.ApproveProductionRequestAsync(productionRequest.Id, cancellationTokenSource);
                else
                    return await _productionRequestService.RejectProductionRequestAsync(productionRequest.Id, cancellationTokenSource);
            },
            services: _productionRequestService);
        }
    }
}
