using MicroserviceProject.Services.Business.Departments.IT.Services;
using MicroserviceProject.Services.Business.Departments.IT.Util.Validation.Inventory.AssignInventoryToWorker;
using MicroserviceProject.Services.Business.Departments.IT.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using MicroserviceProject.Services.Business.Departments.IT.Util.Validation.Inventory.CreateInventory;
using MicroserviceProject.Services.Model.Department.HR;
using MicroserviceProject.Services.Model.Department.IT;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.IT.Controllers
{
    [Authorize]
    [Route("Inventory")]
    public class InventoryController : Controller
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Route(nameof(GetInventories))]
        public async Task<IActionResult> GetInventories(CancellationToken cancellationToken)
        {
            return await ServiceExecuter.ExecuteServiceAsync<List<InventoryModel>>(async () =>
            {
                return await _inventoryService.GetInventoriesAsync(cancellationToken);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(CreateInventory))]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryModel inventory, CancellationToken cancellationToken)
        {
            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await CreateInventoryValidator.ValidateAsync(inventory, cancellationToken);

                return await _inventoryService.CreateInventoryAsync(inventory, cancellationToken);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(AssignInventoryToWorker))]
        public async Task<IActionResult> AssignInventoryToWorker([FromBody] WorkerModel worker, CancellationToken cancellationToken)
        {
            return await ServiceExecuter.ExecuteServiceAsync<WorkerModel>(async () =>
            {
                await AssignInventoryToWorkerValidator.ValidateAsync(worker, cancellationToken);

                return await _inventoryService.AssignInventoryToWorkerAsync(worker, cancellationToken);
            },
            services: _inventoryService);
        }

        [HttpPost]
        [Route(nameof(CreateDefaultInventoryForNewWorker))]
        public async Task<IActionResult> CreateDefaultInventoryForNewWorker([FromBody] InventoryModel inventory, CancellationToken cancellationToken)
        {
            return await ServiceExecuter.ExecuteServiceAsync<InventoryModel>(async () =>
            {
                await CreateDefaultInventoryForNewWorkerValidator.ValidateAsync(inventory, cancellationToken);

                return await _inventoryService.CreateDefaultInventoryForNewWorkerAsync(inventory, cancellationToken);
            },
            services: _inventoryService);
        }

        [HttpGet]
        [Route(nameof(GetInventoriesForNewWorker))]
        public IActionResult GetInventoriesForNewWorker(CancellationToken cancellationToken)
        {
            return ServiceExecuter.ExecuteService<List<InventoryModel>>(() =>
            {
                return _inventoryService.GetInventoriesForNewWorker(cancellationToken);
            },
            services: _inventoryService);
        }
    }
}
