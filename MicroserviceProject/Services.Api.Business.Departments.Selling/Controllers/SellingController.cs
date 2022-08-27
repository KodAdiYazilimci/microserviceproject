using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Selling.Services;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Queries.Responses;

using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Selling.Controllers
{
    [Route("Selling")]
    public class SellingController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly SellingService _sellingService;

        public SellingController(
            IMediator mediator,
            SellingService sellingService)
        {
            _sellingService = sellingService;
            _mediator = mediator;
        }

        [HttpGet]
        [Route(nameof(GetSolds))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetSolds()
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                GetSoldsQueryResponse mediatorResult = await _mediator.Send(new GetSoldsQueryRequest());

                return mediatorResult.Solds;
            },
            services: _sellingService);
        }

        [HttpPost]
        [Route(nameof(CreateSelling))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateSelling([FromBody] CreateSellingCommandRequest createSellingCommandRequest)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(createSellingCommandRequest);
            },
            services: _sellingService);
        }

        [HttpPost]
        [Route(nameof(NotifyProductionRequest))]
        [Authorize(Roles = "ApiUser,QueueUser")]
        public async Task<IActionResult> NotifyProductionRequest([FromBody] NotifyProductionRequestCommandRequest productionRequest)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(productionRequest);
            },
            services: _sellingService);
        }
    }
}
