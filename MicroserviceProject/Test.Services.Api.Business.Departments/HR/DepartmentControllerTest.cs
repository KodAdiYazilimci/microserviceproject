using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.HR.Controllers;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.Models;

using Test.Services.Api.Business.Departments.HR.Factories.Services;

namespace Test.Services.Api.Business.Departments.HR
{
    public class DepartmentControllerTest
    {
        private readonly DepartmentController departmentController;

        public DepartmentControllerTest()
        {
            departmentController = new DepartmentController(null, DepartmentServiceFactory.Instance);
            departmentController.ByPassMediatR = true;
        }

        public async Task<List<DepartmentModel>> GetDepartmentsAsync()
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

        public async Task<ServiceResultModel> CreateDepartmentAsync(CreateDepartmentCommandRequest createDepartmentCommandRequest)
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
    }
}
