using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.IT.Services;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.AssignInventoryToWorker;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.CreateInventory;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.InformInventoryRequest;
using Services.Communication.Http.Broker.Department.IT.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Controllers
{
    [Route("Inventory")]
    public class InventoryController : BaseController
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Route(nameof(GetInventories))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetInventories(CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<InventoryModel>>(async () =>
            {
                return await _inventoryService.GetInventoriesAsync(cancellationTokenSource);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(CreateInventory))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryModel inventory, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateInventoryValidator.ValidateAsync(inventory, cancellationTokenSource);

                return await _inventoryService.CreateInventoryAsync(inventory, cancellationTokenSource);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(AssignInventoryToWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> AssignInventoryToWorker([FromBody] WorkerModel worker, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<WorkerModel>(async () =>
            {
                await AssignInventoryToWorkerValidator.ValidateAsync(worker, cancellationTokenSource);

                return await _inventoryService.AssignInventoryToWorkerAsync(worker, cancellationTokenSource);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(CreateDefaultInventoryForNewWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateDefaultInventoryForNewWorker([FromBody] InventoryModel inventory, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<InventoryModel>(async () =>
            {
                await CreateDefaultInventoryForNewWorkerValidator.ValidateAsync(inventory, cancellationTokenSource);

                return await _inventoryService.CreateDefaultInventoryForNewWorkerAsync(inventory, cancellationTokenSource);
            },
            services: _inventoryService);
        }

        [HttpGet]
        [Route(nameof(GetInventoriesForNewWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public IActionResult GetInventoriesForNewWorker(CancellationTokenSource cancellationTokenSource)
        {
            return HttpResponseWrapper.Wrap<List<InventoryModel>>(() =>
            {
                return _inventoryService.GetInventoriesForNewWorker(cancellationTokenSource);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(InformInventoryRequest))]
        [Authorize(Roles = "ApiUser,QueueUser")]
        public async Task<IActionResult> InformInventoryRequest([FromBody] InventoryRequestModel inventoryRequest, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await InformInventoryRequestValidator.ValidateAsync(inventoryRequest, cancellationTokenSource);

                await _inventoryService.InformInventoryRequestAsync(inventoryRequest, cancellationTokenSource);
            },
            services: _inventoryService);
        }
    }
}
