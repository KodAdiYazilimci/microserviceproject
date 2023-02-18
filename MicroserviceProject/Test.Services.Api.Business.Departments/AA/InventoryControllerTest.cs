using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.AA.Controllers;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.Models;

using StackExchange.Redis;

using Test.Services.Api.Business.Departments.AA.Factories.Services;

namespace Test.Services.Api.Business.Departments.AA
{
    public class InventoryControllerTest
    {
        private readonly InventoryController inventoryController;

        public InventoryControllerTest()
        {
            //IMediator mediator = MediatorFactory.GetInstance(typeof(global::Services.Api.Business.Departments.AA.Program));

            inventoryController = new InventoryController(null, InventoryServiceFactory.Instance);
            inventoryController.ByPassMediatR = true;
        }

        public async Task<List<InventoryModel>> GetInventoriesAsync()
        {
            IActionResult actionResult = await inventoryController.GetInventories();

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                var inventories = okObjectResult.Value as ServiceResultModel<List<InventoryModel>>;

                return inventories.Data;
            }

            return null;
        }

        public async Task<ServiceResultModel> CreateInventoryAsync(CreateInventoryCommandRequest createInventoryCommandRequest)
        {
            IActionResult actionResult = await inventoryController.CreateInventory(createInventoryCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return okObjectResult.Value as ServiceResultModel;
            }

            return null;
        }
    }
}
