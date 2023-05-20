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
    public class PersonControllerTest
    {
        private readonly RuntimeHandler runtimeHandler;
        private readonly PersonService personService;
        private readonly PersonController personController;

        public PersonControllerTest()
        {
            runtimeHandler = RuntimeHandlerFactory.GetInstance(
                runtimeLogger: RuntimeLoggerFactory.GetInstance(
                    configuration: ConfigurationFactory.GetConfiguration()));

            personService = PersonServiceFactory.Instance;
            personController = new PersonController(null, personService);
            personController.ByPassMediatR = true;
        }

        public async Task<List<WorkerModel>> GetWorkersAsync(bool byPassMediatR = true)
        {
            if (byPassMediatR)
            {
                IActionResult actionResult = await personController.GetWorkers();

                if (actionResult is OkObjectResult)
                {
                    OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                    var workers = okObjectResult.Value as ServiceResultModel<List<WorkerModel>>;

                    return workers.Data;
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
                var response = MediatorFactory.GetInstance<GetWorkersQueryRequest, GetWorkersQueryResponse>(
                    request: new GetWorkersQueryRequest(),
                    requestHandler: new GetWorkersQueryHandler(
                        runtimeHandler: runtimeHandler,
                        personService: personService));

                return response.Workers;
            }
        }

        public async Task<ServiceResultModel> CreateWorkerAsync(CreateWorkerCommandRequest createWorkerCommandRequest, bool byPassMediatR = true)
        {
            if (byPassMediatR)
            {
                IActionResult actionResult = await personController.CreateWorker(createWorkerCommandRequest);

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
                var response = MediatorFactory.GetInstance<CreateWorkerCommandRequest, CreateWorkerCommandResponse>(
                    request: createWorkerCommandRequest,
                    requestHandler: new CreateWorkerCommandHandler(
                        runtimeHandler: runtimeHandler,
                        personService: personService));

                return new ServiceResultModel() { IsSuccess = response.CreatedWorkerId > 0 };
            }
        }

        public async Task<ServiceResultModel> CreatePersonAsync(CreatePersonCommandRequest createPersonCommandRequest, bool byPassMediatR = true)
        {
            if (byPassMediatR)
            {
                IActionResult actionResult = await personController.CreatePerson(createPersonCommandRequest);

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
                var response = MediatorFactory.GetInstance<CreatePersonCommandRequest, CreatePersonCommandResponse>(
                    request: createPersonCommandRequest,
                    requestHandler: new CreatePersonCommandHandler(
                        runtimeHandler: runtimeHandler,
                        personService: personService));

                return new ServiceResultModel() { IsSuccess = response.CreatedPersonId > 0 };
            }
        }

        public async Task<List<PersonModel>> GetPeopleAsync(bool byPassMediatR = true)
        {
            if (byPassMediatR)
            {
                IActionResult actionResult = await personController.GetPeople();

                if (actionResult is OkObjectResult)
                {
                    OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                    return (okObjectResult.Value as ServiceResultModel<List<PersonModel>>).Data;
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
                var response = MediatorFactory.GetInstance<GetPeopleQueryRequest, GetPeopleQueryResponse>(
                    request: new GetPeopleQueryRequest(),
                    requestHandler: new GetPeopleQueryHandler(
                        runtimeHandler: runtimeHandler,
                        personService: personService));

                return response.People;
            }
        }

        public async Task<List<TitleModel>> GetTitles(bool byPassMediatR = true)
        {
            if (byPassMediatR)
            {
                IActionResult actionResult = await personController.GetTitles();

                if (actionResult is OkObjectResult)
                {
                    OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                    var titles = okObjectResult.Value as ServiceResultModel<List<TitleModel>>;

                    return titles.Data;
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
                var response = MediatorFactory.GetInstance<GetTitlesQueryRequest, GetTitlesQueryResponse>(
                    request: new GetTitlesQueryRequest(),
                    requestHandler: new GetTitlesQueryHandler(
                        runtimeHandler: runtimeHandler,
                        personService: personService));

                return response.Titles;
            }
        }

        public async Task<ServiceResultModel> CreateTitle(CreateTitleCommandRequest createTitleCommandRequest, bool byPassMediatR = true)
        {
            if (byPassMediatR)
            {
                IActionResult actionResult = await personController.CreateTitle(createTitleCommandRequest);

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
                var response = MediatorFactory.GetInstance<CreateTitleCommandRequest, CreateTitleCommandResponse>(
                    request: createTitleCommandRequest,
                    requestHandler: new CreateTitleCommandHandler(
                        runtimeHandler: runtimeHandler,
                        personService: personService));

                return new ServiceResultModel() { IsSuccess = response.CreatedTitleId > 0 };
            }
        }
    }
}
