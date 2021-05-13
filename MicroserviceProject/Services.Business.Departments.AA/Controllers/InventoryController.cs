using Infrastructure.Communication.Model.Department.AA;
using Infrastructure.Communication.Model.Department.Buying;
using Infrastructure.Communication.Model.Department.HR;
using Infrastructure.Transaction.ExecutionHandler;
using Services.Business.Departments.AA.Services;
using Services.Business.Departments.AA.Util.Validation.Inventory.AssignInventoryToWorker;
using Services.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Business.Departments.AA.Util.Validation.Inventory.CreateInventory;
using Services.Business.Departments.AA.Util.Validation.Inventory.InformInventoryRequest;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.AA.Controllers
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
            SetServiceDefaults(_inventoryService);

            return await ServiceExecuter.ExecuteServiceAsync<List<InventoryModel>>(async () =>
            {
                return await _inventoryService.GetInventoriesAsync(cancellationTokenSource);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(CreateInventory))]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryModel inventory, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_inventoryService);

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
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
            SetServiceDefaults(_inventoryService);

            return await ServiceExecuter.ExecuteServiceAsync<WorkerModel>(async () =>
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
            SetServiceDefaults(_inventoryService);

            return await ServiceExecuter.ExecuteServiceAsync<InventoryModel>(async () =>
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
            SetServiceDefaults(_inventoryService);

            return ServiceExecuter.ExecuteService<List<InventoryModel>>(() =>
            {
                return _inventoryService.GetInventoriesForNewWorker(cancellationTokenSource);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(InformInventoryRequest))]
        public async Task<IActionResult> InformInventoryRequest([FromBody] InventoryRequestModel inventoryRequest, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_inventoryService);

            return await ServiceExecuter.ExecuteServiceAsync(async () =>
            {
                await InformInventoryRequestValidator.ValidateAsync(inventoryRequest, cancellationTokenSource);

                await _inventoryService.InformInventoryRequestAsync(inventoryRequest, cancellationTokenSource);
            },
            services: _inventoryService);
        }
    }
}
