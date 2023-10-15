using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.AA.Services;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.AssignInventoryToWorker;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.InformInventoryRequest;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Requests;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Controllers
{
    [Route("Inventory")]
    public class InventoryController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly InventoryService _inventoryService;
        private readonly CreateDefaultInventoryForNewWorkerValidator _createDefaultInventoryForNewWorkerValidator;
        private readonly InformInventoryRequestValidator _informInventoryRequestValidator;

        public InventoryController(
            IMediator mediator,
            InventoryService inventoryService,
            CreateDefaultInventoryForNewWorkerValidator createDefaultInventoryForNewWorkerValidator,
            InformInventoryRequestValidator informInventoryRequestValidator)
        {
            _mediator = mediator;
            _inventoryService = inventoryService;
            _createDefaultInventoryForNewWorkerValidator = createDefaultInventoryForNewWorkerValidator;
            _informInventoryRequestValidator = informInventoryRequestValidator;
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
                    return (await _mediator.Send(new AAGetInventoriesQueryRequest())).Inventories;
            },
            services: _inventoryService);
        }

        [HttpPut]
        [Route(nameof(CreateInventory))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateInventory([FromBody] AACreateInventoryCommandRequest request)
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
        public async Task<IActionResult> AssignInventoryToWorker([FromBody] AAAssignInventoryToWorkerCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                    await AssignInventoryToWorkerValidator.ValidateAsync(request.AssignInventoryToWorkerModels, cancellationTokenSource);

                    await _inventoryService.AssignInventoryToWorkerAsync(request.AssignInventoryToWorkerModels, cancellationTokenSource);
                }
                else
                    await _mediator.Send(request);
            },
            services: _inventoryService);
        }

        [HttpPut]
        [Route(nameof(CreateDefaultInventoryForNewWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateDefaultInventoryForNewWorker([FromBody] AACreateDefaultInventoryForNewWorkerCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                    await _createDefaultInventoryForNewWorkerValidator.ValidateAsync(request.DefaultInventoryForNewWorkerModel, cancellationTokenSource);

                    await _inventoryService.CreateDefaultInventoryForNewWorkerAsync(request.DefaultInventoryForNewWorkerModel, cancellationTokenSource);
                }
                else
                    await _mediator.Send(request);
            },
            services: _inventoryService);
        }

        [HttpGet]
        [Route(nameof(GetInventoriesForNewWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> GetInventoriesForNewWorker()
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    return _inventoryService.GetInventoriesForNewWorker(new CancellationTokenSource());
                else
                    return (await _mediator.Send(new AAGetInventoriesForNewWorkerQueryRequest())).Inventories;
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(InformInventoryRequest))]
        [Authorize(Roles = "ApiUser,QueueUser")]
        public async Task<IActionResult> InformInventoryRequest([FromBody] AAInformInventoryRequestCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                    await _informInventoryRequestValidator.ValidateAsync(request.InventoryRequest, cancellationTokenSource);

                    await _inventoryService.InformInventoryRequestAsync(request.InventoryRequest, cancellationTokenSource);
                }
                else
                    await _mediator.Send(request);
            },
            services: _inventoryService);
        }
    }
}
