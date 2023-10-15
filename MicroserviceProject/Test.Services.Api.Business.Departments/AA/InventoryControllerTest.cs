using Infrastructure.Communication.Http.Models;
using Infrastructure.Mock.Factories;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.CommandHandlers;
using Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.QueryHandlers;
using Services.Api.Business.Departments.AA.Controllers;
using Services.Api.Business.Departments.AA.Services;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.AA.Models;
using Services.Logging.Aspect.Handlers;
using Services.Runtime.Aspect.Mock;

using Test.Services.Api.Business.Departments.AA.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.AA.Factories.Services;

namespace Test.Services.Api.Business.Departments.AA
{
    public class InventoryControllerTest
    {
        private readonly RuntimeHandler runtimeHandler;
        private readonly InventoryService inventoryService;
        private readonly InventoryController inventoryController;

        public InventoryControllerTest()
        {
            runtimeHandler = RuntimeHandlerFactory.GetInstance(
                runtimeLogger: RuntimeLoggerFactory.GetInstance(
                    configuration: ConfigurationFactory.GetConfiguration()));

            inventoryService = InventoryServiceFactory.Instance;
            inventoryController =
                new InventoryController(
                    mediator: null,
                    inventoryService: inventoryService,
                    createDefaultInventoryForNewWorkerValidator: new global::Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker.CreateDefaultInventoryForNewWorkerValidator(
                        validationRule: new global::Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.CreateDefaultInventoryForNewWorker.CreateDefaultInventoryForNewWorkerRule()),
                    informInventoryRequestValidator: new global::Services.Api.Business.Departments.AA.Util.Validation.Inventory.InformInventoryRequest.InformInventoryRequestValidator(
                        validationRule: new global::Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.InformInventoryRequest.InformInventoryRequestRule()));
            inventoryController.ByPassMediatR = true;
        }

        public async Task<List<AAInventoryModel>> GetInventoriesAsync(bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AAGetInventoriesQueryRequest, AAGetInventoriesQueryResponse>(
                    request: new AAGetInventoriesQueryRequest(),
                    requestHandler: new GetInventoriesQueryHandler(
                        runtimeHandler: runtimeHandler,
                        inventoryService: inventoryService));

                return response.Inventories;
            }
        }

        public async Task<ServiceResultModel> CreateInventoryAsync(AACreateInventoryCommandRequest createInventoryCommandRequest, bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AACreateInventoryCommandRequest, AACreateInventoryCommandResponse>(
                    request: createInventoryCommandRequest,
                    requestHandler: new CreateInventoryCommandHandler(
                        runtimeHandler: runtimeHandler,
                        inventoryService: inventoryService,
                        createInventoryValidator: new global::Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateInventory.CreateInventoryValidator(
                            validationRule: new global::Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.CreateInventory.CreateInventoryRule())));

                return new ServiceResultModel() { IsSuccess = response.CreatedInventoryId > 0 };
            }
        }

        public async Task<ServiceResultModel> AssignInventoryToWorkerTest(AAAssignInventoryToWorkerCommandRequest assignInventoryToWorkerCommandRequest, bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AAAssignInventoryToWorkerCommandRequest, AAAssignInventoryToWorkerCommandResponse>(
                    request: assignInventoryToWorkerCommandRequest,
                    requestHandler: new AssignInventoryToWorkerCommandHandler(
                        runtimeHandler: runtimeHandler,
                        inventoryService: InventoryServiceFactory.Instance));

                return new ServiceResultModel() { IsSuccess = response != null };
            }
        }

        public async Task<ServiceResultModel> CreateDefaultInventoryForNewWorker(AACreateDefaultInventoryForNewWorkerCommandRequest createDefaultInventoryForNewWorkerCommandRequest,
            bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AACreateDefaultInventoryForNewWorkerCommandRequest, AACreateDefaultInventoryForNewWorkerCommandResponse>(
                    request: createDefaultInventoryForNewWorkerCommandRequest,
                    requestHandler: new CreateDefaultInventoryForNewWorkerCommandHandler(
                        runtimeHandler: runtimeHandler,
                        inventoryService: inventoryService,
                        createDefaultInventoryForNewWorkerValidator: new global::Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker.CreateDefaultInventoryForNewWorkerValidator(
                            validationRule: new global::Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.CreateDefaultInventoryForNewWorker.CreateDefaultInventoryForNewWorkerRule())));

                return new ServiceResultModel() { IsSuccess = response != null };
            }
        }

        public async Task<List<AADefaultInventoryForNewWorkerModel>> GetInventoriesForNewWorker(bool byPassMediatR = true)
        {
            if (byPassMediatR)
            {
                IActionResult actionResult = await inventoryController.GetInventoriesForNewWorker();

                if (actionResult is OkObjectResult)
                {
                    OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                    return (okObjectResult.Value as ServiceResultModel<List<AADefaultInventoryForNewWorkerModel>>).Data;
                }
                else if (actionResult is BadRequestObjectResult)
                {
                    BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                    throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
                }

                return null;
            }
            else
            {
                var response = MediatorFactory.GetInstance<AAGetInventoriesForNewWorkerQueryRequest, AAGetInventoriesForNewWorkerQueryResponse>(
                    request: new AAGetInventoriesForNewWorkerQueryRequest(),
                    requestHandler: new GetInventoriesForNewWorkerQueryHandler(
                        runtimeHandler: runtimeHandler,
                        inventoryService: inventoryService));

                return response.Inventories;
            }
        }

        public async Task<ServiceResultModel> InformInventoryRequest(AAInformInventoryRequestCommandRequest informInventoryRequestCommandRequest, bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AAInformInventoryRequestCommandRequest, AAInformInventoryRequestCommandResponse>(
                    request: informInventoryRequestCommandRequest,
                    requestHandler: new InformInventoryRequestCommandHandler(
                        runtimeHandler: runtimeHandler,
                        inventoryService: inventoryService,
                        informInventoryRequestValidator: new global::Services.Api.Business.Departments.AA.Util.Validation.Inventory.InformInventoryRequest.InformInventoryRequestValidator(
                            validationRule: new global::Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.InformInventoryRequest.InformInventoryRequestRule())));

                return new ServiceResultModel() { IsSuccess = response != null };
            }
        }
    }
}
