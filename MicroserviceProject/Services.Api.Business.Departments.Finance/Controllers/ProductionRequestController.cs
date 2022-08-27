using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Business.Departments.Finance.Services;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Responses;

using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Controllers
{
    [Route("ProductionRequest")]
    public class ProductionRequestController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ProductionRequestService _productionRequestService;

        public ProductionRequestController(
            IMediator mediator,
            ProductionRequestService productionRequestService)
        {
            _mediator = mediator;
            _productionRequestService = productionRequestService;
        }

        [HttpGet]
        [Route(nameof(GetProductionRequests))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetProductionRequests()
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                GetProductionRequestsQueryResponse mediatorResult = await _mediator.Send(new GetProductionRequestsQueryRequest());

                return mediatorResult.ProductionRequests;
            },
            services: _productionRequestService);
        }

        [Route(nameof(CreateProductionRequest))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateProductionRequest([FromBody] CreateProductionRequestCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(request);
            },
            services: _productionRequestService);
        }


        [HttpPost]
        [Route(nameof(DecideProductionRequest))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> DecideProductionRequest([FromBody] DecideProductionRequestCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(request);
            },
            services: _productionRequestService);
        }
    }
}
