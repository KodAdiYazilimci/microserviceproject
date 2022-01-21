
using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;

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
            return await HttpResponseWrapper.WrapAsync<GetPeopleQueryResponse>(async () =>
            {
                return await _mediator.Send(new GetPeopleQueryRequest());
            },
            services: _personService);
        }

        [HttpPost]
        [Route(nameof(CreatePerson))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync<CreatePersonCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _personService);
        }

        [HttpGet]
        [Route(nameof(GetTitles))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetTitles()
        {
            return await HttpResponseWrapper.WrapAsync<GetTitlesQueryResponse>(async () =>
            {
                return await _mediator.Send(new GetTitlesQueryRequest());
            },
            services: _personService);
        }

        [HttpPost]
        [Route(nameof(CreateTitle))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateTitle([FromBody] CreateTitleCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync<CreateTitleCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _personService);
        }

        [HttpGet]
        [Route(nameof(GetWorkers))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetWorkers()
        {
            return await HttpResponseWrapper.WrapAsync<GetWorkersQueryResponse>(async () =>
            {
                return await _mediator.Send(new GetWorkersQueryRequest());
            },
            services: _personService);
        }

        [HttpPost]
        [Route(nameof(CreateWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateWorker([FromBody] CreateWorkerCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync<CreateWorkerCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _personService);
        }
    }
}
