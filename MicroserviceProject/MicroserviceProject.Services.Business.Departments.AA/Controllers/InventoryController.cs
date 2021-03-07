using MicroserviceProject.Services.Business.Departments.AA.Services;
using MicroserviceProject.Services.Business.Departments.AA.Util.Validation.Inventory.AssignInventoryToWorker;
using MicroserviceProject.Services.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using MicroserviceProject.Services.Business.Departments.AA.Util.Validation.Inventory.CreateInventory;
using MicroserviceProject.Services.Business.Departments.AA.Util.Validation.Inventory.InformInventoryRequest;
using MicroserviceProject.Services.Model.Department.AA;
using MicroserviceProject.Services.Model.Department.Buying;
using MicroserviceProject.Services.Model.Department.HR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.AA.Controllers
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
        public async Task<IActionResult> GetInventories(CancellationTokenSource cancellationTokenSource)
        {
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _inventoryService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

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
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _inventoryService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

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
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _inventoryService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

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
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _inventoryService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

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
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _inventoryService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

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
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _inventoryService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync(async () =>
            {
                await InformInventoryRequestValidator.ValidateAsync(inventoryRequest, cancellationTokenSource);

                await _inventoryService.InformInventoryRequestAsync(inventoryRequest, cancellationTokenSource);
            },
            services: _inventoryService);
        }
    }
}
