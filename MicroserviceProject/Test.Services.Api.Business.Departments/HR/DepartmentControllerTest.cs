using Infrastructure.Communication.Http.Models;
using Infrastructure.Mock.Factories;

using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.CommandHandlers;
using Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.QueryHandlers;
using Services.Api.Business.Departments.HR.Controllers;
using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.HR.Models;
using Services.Logging.Aspect.Handlers;
using Services.Runtime.Aspect.Mock;

using Test.Services.Api.Business.Departments.HR.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.HR.Factories.Services;

namespace Test.Services.Api.Business.Departments.HR
{
    public class DepartmentControllerTest
    {
        private readonly RuntimeHandler runtimeHandler;
        private readonly DepartmentService departmentService;
        private readonly DepartmentController departmentController;

        public DepartmentControllerTest()
        {
            runtimeHandler = RuntimeHandlerFactory.GetInstance(
                runtimeLogger: RuntimeLoggerFactory.GetInstance(
                    configuration: ConfigurationFactory.GetConfiguration()));

            departmentService = DepartmentServiceFactory.Instance;
            departmentController = new DepartmentController(null, departmentService);
            departmentController.ByPassMediatR = true;
        }

        public async Task<List<DepartmentModel>> GetDepartmentsAsync(bool byPassMediatR = true)
        {
            if (byPassMediatR)
            {
                IActionResult actionResult = await departmentController.GetDepartments();

                if (actionResult is OkObjectResult)
                {
                    OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                    var inventories = okObjectResult.Value as ServiceResultModel<List<DepartmentModel>>;

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
                var response = MediatorFactory.GetInstance<GetDepartmentsQueryRequest, GetDepartmentsQueryResponse>(
                    request: new GetDepartmentsQueryRequest(),
                    requestHandler: new GetDepartmentsQueryHandler(
                        runtimeHandler: runtimeHandler,
                        departmentService: departmentService));

                return response.Departments;
            }
        }

        public async Task<ServiceResultModel> CreateDepartmentAsync(CreateDepartmentCommandRequest createDepartmentCommandRequest, bool byPassMediatR = true)
        {
            if (byPassMediatR)
            {
                IActionResult actionResult = await departmentController.CreateDepartment(createDepartmentCommandRequest);

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
                var response = MediatorFactory.GetInstance<CreateDepartmentCommandRequest, CreateDepartmentCommandResponse>(
                    request: createDepartmentCommandRequest,
                    requestHandler: new CreateDepartmentCommandHandler(
                        runtimeHandler: runtimeHandler,
                        departmentService: departmentService));

                return new ServiceResultModel() { IsSuccess = response.CreatedDepartmentId > 0 };
            }
        }
    }
}