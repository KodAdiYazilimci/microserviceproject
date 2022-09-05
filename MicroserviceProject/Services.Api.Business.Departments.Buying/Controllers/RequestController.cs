using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Buying.Services;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Queries.Responses;

using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Controllers
{
    [Route("Request")]
    public class RequestController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly RequestService _requestService;

        public RequestController(RequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        [Route(nameof(GetInventoryRequests))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetInventoryRequests()
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                GetInventoryRequestsQueryResponse mediatorResult = await _mediator.Send(new GetInventoryRequestsQueryRequest());

                return mediatorResult.InventoryRequests;
            },
            services: _requestService);
        }

        [HttpPost]
        [Route(nameof(CreateInventoryRequest))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateInventoryRequest([FromBody] CreateInventoryRequestCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(request);
            },
            services: _requestService);
        }

        [HttpPost]
        [Route(nameof(ValidateCostInventory))]
        [Authorize(Roles = "ApiUser,QueueUser")]
        public async Task<IActionResult> ValidateCostInventory([FromBody] ValidateCostInventoryCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(request);
            },
            services: _requestService);
        }
    }
}
