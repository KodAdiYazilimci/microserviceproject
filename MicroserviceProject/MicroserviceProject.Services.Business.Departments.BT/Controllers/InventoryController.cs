using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Services.Business.Departments.IT.Services;
using MicroserviceProject.Services.Business.Departments.IT.Util.Validation.Inventory.AssignInventoryToWorker;
using MicroserviceProject.Services.Business.Departments.IT.Util.Validation.Inventory.CreateInventory;
using MicroserviceProject.Services.Model.Department.HR;
using MicroserviceProject.Services.Model.Department.IT;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
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
            try
            {
                List<InventoryModel> departments =
                    await _inventoryService.GetInventoriesAsync(cancellationToken);

                ServiceResult<List<InventoryModel>> serviceResult = new ServiceResult<List<InventoryModel>>()
                {
                    IsSuccess = true,
                    Data = departments
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpPost]
        [Route(nameof(CreateInventory))]
        public async Task<IActionResult> CreateInventory(
            [FromBody] InventoryModel inventory,
            CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult =
                    await CreateInventoryValidator.ValidateAsync(inventory, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                int generatedId = await _inventoryService.CreateInventoryAsync(inventory, cancellationToken);

                ServiceResult<int> serviceResult = new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = generatedId
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpPost]
        [Route(nameof(AssignInventoryToWorker))]
        public async Task<IActionResult> AssignInventoryToWorker([FromBody] WorkerModel worker, CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult =
                    await AssignInventoryToWorkerValidator.ValidateAsync(worker, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                WorkerModel generatedWorker = await _inventoryService.AssignInventoryToWorkerAsync(worker, cancellationToken);

                ServiceResult<WorkerModel> serviceResult = new ServiceResult<WorkerModel>()
                {
                    IsSuccess = true,
                    Data = generatedWorker
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpGet]
        [Route(nameof(GetInventoriesForNewWorker))]
        public IActionResult GetInventoriesForNewWorker(CancellationToken cancellationToken)
        {
            try
            {
                List<InventoryModel> inventories =
                    _inventoryService.GetInventoriesForNewWorker(cancellationToken);

                ServiceResult<List<InventoryModel>> serviceResult = new ServiceResult<List<InventoryModel>>()
                {
                    IsSuccess = true,
                    Data = inventories
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }
    }
}
