using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.AA.Services;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Controllers
{
    [Route("Inventory")]
    public class InventoryController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly InventoryService _inventoryService;

        public InventoryController(
            IMediator mediator,
            InventoryService inventoryService)
        {
            _mediator = mediator;
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Route(nameof(GetInventories))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetInventories()
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    return await _inventoryService.GetInventoriesAsync(new CancellationTokenSource());
                else
                    return (await _mediator.Send(new GetInventoriesQueryRequest())).Inventories;
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(CreateInventory))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateInventory([FromBody] CreateInventoryCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    await _inventoryService.CreateInventoryAsync(request.Inventory, new CancellationTokenSource());
                else
                    await _mediator.Send(request);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(AssignInventoryToWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> AssignInventoryToWorker([FromBody] AssignInventoryToWorkerCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    await _inventoryService.AssignInventoryToWorkerAsync(request.Worker, new CancellationTokenSource());
                else
                    await _mediator.Send(request);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(CreateDefaultInventoryForNewWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateDefaultInventoryForNewWorker([FromBody] CreateDefaultInventoryForNewWorkerCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    await _inventoryService.CreateDefaultInventoryForNewWorkerAsync(request.Inventory, new CancellationTokenSource());
                else
                    await _mediator.Send(request);
            },
            services: _inventoryService);
        }

        [HttpGet]
        [Route(nameof(GetInventoriesForNewWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public IActionResult GetInventoriesForNewWorker()
        {
            return HttpResponseWrapper.Wrap(async () =>
            {
                if (ByPassMediatR)
                    return _inventoryService.GetInventoriesForNewWorker(new CancellationTokenSource());
                else
                    return (await _mediator.Send(new GetInventoriesForNewWorkerQueryRequest())).Inventories;
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
                if (ByPassMediatR)
                    await _inventoryService.InformInventoryRequestAsync(request.InventoryRequest, new CancellationTokenSource());
                else 
                    await _mediator.Send(request);
            },
            services: _inventoryService);
        }
    }
}
