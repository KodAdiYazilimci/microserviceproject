using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.IT.Services;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.AssignInventoryToWorker;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.CreateInventory;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.InformInventoryRequest;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.IT.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Controllers
{
    [Route("Inventory")]
    public class InventoryController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly InventoryService _inventoryService;

        public InventoryController(IMediator mediator, InventoryService inventoryService)
        {
            _mediator = mediator;
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Route(nameof(GetInventories))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetInventories()
        {
            return await HttpResponseWrapper.WrapAsync<GetInventoriesQueryResponse>(async () =>
            {
                return await _mediator.Send(new GetInventoriesQueryRequest());
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(CreateInventory))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateInventory([FromBody] CreateInventoryCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync<CreateInventoryCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(AssignInventoryToWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> AssignInventoryToWorker([FromBody] AssignInventoryToWorkerCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync<AssignInventoryToWorkerCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(CreateDefaultInventoryForNewWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateDefaultInventoryForNewWorker([FromBody] CreateDefaultInventoryForNewWorkerCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync<CreateDefaultInventoryForNewWorkerCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _inventoryService);
        }

        [HttpGet]
        [Route(nameof(GetInventoriesForNewWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public IActionResult GetInventoriesForNewWorker()
        {
            return HttpResponseWrapper.Wrap<GetInventoriesForNewWorkerQueryResponse>(() =>
            {
                return _mediator.Send(new GetInventoriesForNewWorkerQueryRequest()).Result;
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(InformInventoryRequest))]
        [Authorize(Roles = "ApiUser,QueueUser")]
        public async Task<IActionResult> InformInventoryRequest([FromBody] InformInventoryRequestCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(request);
            },
            services: _inventoryService);
        }
    }
}
