using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Production.Services;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Controllers
{
    [Route("Production")]
    public class ProductionController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ProductionService _productionService;

        public ProductionController(IMediator mediator, ProductionService productionService)
        {
            _mediator = mediator;
            _productionService = productionService;
        }

        [HttpPost]
        [Route(nameof(ProduceProduct))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> ProduceProduct([FromBody] ProduceProductCommandRequest request, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<ProduceProductCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _productionService);
        }

        [HttpPost]
        [Route(nameof(ReEvaluateProduceProduct))]
        [Authorize(Roles = "ApiUser,QueueUser")]
        public async Task<IActionResult> ReEvaluateProduceProduct(int referenceNumber, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<ReEvaluateProduceProductCommandResponse>(async () =>
            {
                return await _mediator.Send(new ReEvaluateProduceProductCommandRequest() { ReferenceNumber = referenceNumber });
            },
           services: _productionService);
        }
    }
}
