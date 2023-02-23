using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.AA.Controllers;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.Models;

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

        public async Task<List<AAInventoryModel>> GetInventoriesAsync()
        {
            IActionResult actionResult = await inventoryController.GetInventories();

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                var inventories = okObjectResult.Value as ServiceResultModel<List<AAInventoryModel>>;

                return inventories.Data;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }

        public async Task<ServiceResultModel> CreateInventoryAsync(AACreateInventoryCommandRequest createInventoryCommandRequest)
        {
            IActionResult actionResult = await inventoryController.CreateInventory(createInventoryCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return okObjectResult.Value as ServiceResultModel;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }

        public async Task<ServiceResultModel> AssignInventoryToWorkerTest(AAAssignInventoryToWorkerCommandRequest assignInventoryToWorkerCommandRequest)
        {
            IActionResult actionResult = await inventoryController.AssignInventoryToWorker(assignInventoryToWorkerCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return okObjectResult.Value as ServiceResultModel;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }

        public async Task<ServiceResultModel> CreateDefaultInventoryForNewWorker(AACreateDefaultInventoryForNewWorkerCommandRequest createDefaultInventoryForNewWorkerCommandRequest)
        {
            IActionResult actionResult = await inventoryController.CreateDefaultInventoryForNewWorker(createDefaultInventoryForNewWorkerCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                var result = okObjectResult.Value as ServiceResultModel;

                return result;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }

        public List<AAInventoryModel> GetInventoriesForNewWorker()
        {
            IActionResult actionResult = inventoryController.GetInventoriesForNewWorker();

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return (okObjectResult.Value as ServiceResultModel<List<AAInventoryModel>>).Data;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }

        public async Task<ServiceResultModel> InformInventoryRequest(AAInformInventoryRequestCommandRequest informInventoryRequestCommandRequest)
        {
            IActionResult actionResult = await inventoryController.InformInventoryRequest(informInventoryRequestCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return okObjectResult.Value as ServiceResultModel;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }
    }
}
