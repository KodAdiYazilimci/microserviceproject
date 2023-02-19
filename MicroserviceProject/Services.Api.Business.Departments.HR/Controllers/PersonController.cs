
using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Controllers
{
    [Route("Person")]
    public class PersonController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly PersonService _personService;

        public PersonController(IMediator mediator, PersonService personService)
        {
            _mediator = mediator;
            _personService = personService;
        }

        [HttpGet]
        [Route(nameof(GetPeople))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetPeople()
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    return await _personService.GetPeopleAsync(new CancellationTokenSource());
                else
                {
                    GetPeopleQueryResponse mediatorResult = await _mediator.Send(new GetPeopleQueryRequest());

                    return mediatorResult.People;
                }
            },
            services: _personService);
        }

        [HttpPost]
        [Route(nameof(CreatePerson))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    await _personService.CreatePersonAsync(request.Person, new CancellationTokenSource());
                else
                    await _mediator.Send(request);
            },
            services: _personService);
        }

        [HttpGet]
        [Route(nameof(GetTitles))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetTitles()
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    return await _personService.GetTitlesAsync(new CancellationTokenSource());
                else
                {
                    GetTitlesQueryResponse mediatorResult = await _mediator.Send(new GetTitlesQueryRequest());

                    return mediatorResult.Titles;
                }
            },
            services: _personService);
        }

        [HttpPost]
        [Route(nameof(CreateTitle))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateTitle([FromBody] CreateTitleCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    await _personService.CreateTitleAsync(request.Title, new CancellationTokenSource());
                else
                    await _mediator.Send(request);
            },
            services: _personService);
        }

        [HttpGet]
        [Route(nameof(GetWorkers))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetWorkers()
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    return await _personService.GetWorkersAsync(new CancellationTokenSource());
                else
                {
                    GetWorkersQueryResponse mediatorResult = await _mediator.Send(new GetWorkersQueryRequest());

                    return mediatorResult.Workers;
                }
            },
            services: _personService);
        }

        [HttpPost]
        [Route(nameof(CreateWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateWorker([FromBody] CreateWorkerCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    await _personService.CreateWorkerAsync(request.Worker, new CancellationTokenSource());
                else
                    await _mediator.Send(request);
            },
            services: _personService);
        }
    }
}
