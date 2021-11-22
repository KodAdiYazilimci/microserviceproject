using Services.Communication.Http.Broker.Department.AA.Models;

using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.AA.Services;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.AssignInventoryToWorker;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateInventory;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.InformInventoryRequest;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Controllers
{
    [Authorize]
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
