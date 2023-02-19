using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.HR.Controllers;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.Models;

using Test.Services.Api.Business.Departments.HR.Factories.Services;

namespace Test.Services.Api.Business.Departments.HR
{
    public class PersonControllerTest
    {
        private readonly PersonController personController;

        public PersonControllerTest()
        {
            personController = new PersonController(null, PersonServiceFactory.Instance);
            personController.ByPassMediatR = true;
        }

        public async Task<List<WorkerModel>> GetWorkersAsync()
        {
            IActionResult actionResult = await personController.GetWorkers();

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                var workers = okObjectResult.Value as ServiceResultModel<List<WorkerModel>>;

                return workers.Data;
            }

            return null;
        }

        public async Task<ServiceResultModel> CreateWorkerAsync(CreateWorkerCommandRequest createWorkerCommandRequest)
        {
            IActionResult actionResult = await personController.CreateWorker(createWorkerCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return okObjectResult.Value as ServiceResultModel;
            }

            return null;
        }

        public async Task<ServiceResultModel> CreatePersonAsync(CreatePersonCommandRequest createPersonCommandRequest)
        {
            IActionResult actionResult = await personController.CreatePerson(createPersonCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return okObjectResult.Value as ServiceResultModel;
            }

            return null;
        }

        public async Task<List<PersonModel>> GetPeopleAsync()
        {
            IActionResult actionResult = await personController.GetPeople();

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return (okObjectResult.Value as ServiceResultModel<List<PersonModel>>).Data;
            }

            return null;
        }

        public async Task<List<TitleModel>> GetTitles()
        {
            IActionResult actionResult = await personController.GetTitles();

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                var titles = okObjectResult.Value as ServiceResultModel<List<TitleModel>>;

                return titles.Data;
            }

            return null;
        }

        public async Task<ServiceResultModel> CreateTitle(CreateTitleCommandRequest createTitleCommandRequest)
        {
            IActionResult actionResult = await personController.CreateTitle(createTitleCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return okObjectResult.Value as ServiceResultModel;
            }

            return null;
        }
    }
}
