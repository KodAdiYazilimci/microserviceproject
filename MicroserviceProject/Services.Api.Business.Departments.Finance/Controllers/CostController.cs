using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Business.Departments.Finance.Services;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Responses;

using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Controllers
{
    [Route("Cost")]
    public class CostController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly CostService _costService;

        public CostController(IMediator mediator, CostService costService)
        {
            _mediator = mediator;
            _costService = costService;
        }

        [HttpGet]
        [Route(nameof(GetDecidedCosts))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetDecidedCosts()
        {
            return await HttpResponseWrapper.WrapAsync<GetDecidedCostsQueryResponse>(async () =>
            {
                return await _mediator.Send(new GetDecidedCostsQueryRequest());
            },
            services: _costService);
        }

        [HttpPost]
        [Route(nameof(CreateCost))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateCost([FromBody] CreateCostCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync<CreateCostCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _costService);
        }

        [HttpPost]
        [Route(nameof(DecideCost))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> DecideCost([FromBody] DecideCostCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync<DecideCostCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _costService);
        }
    }
}
