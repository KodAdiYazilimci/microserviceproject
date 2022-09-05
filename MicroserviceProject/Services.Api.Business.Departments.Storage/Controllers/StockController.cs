using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Storage.Services;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Queries.Responses;

using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Controllers
{
    [Route("Stock")]
    public class StockController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly StockService _stockService;

        public StockController(StockService stockService, IMediator mediator)
        {
            _stockService = stockService;
            _mediator = mediator;
        }

        [HttpGet]
        [Route(nameof(GetStock))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetStock(int productId)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                GetStockQueryResponse mediatorResult = await _mediator.Send(new GetStockQueryRequest() { ProductId = productId });

                return mediatorResult.Stock;
            },
            services: _stockService);
        }

        [HttpPost]
        [Route(nameof(CreateStock))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockCommandRequest createStockCommandRequest)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(createStockCommandRequest);
            },
            services: _stockService);
        }

        [HttpPost]
        [Route(nameof(DescendProductStock))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> DescendProductStock([FromBody] DescendProductStockCommandRequest descendProductStockCommandRequest)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                DescendProductStockCommandResponse result = await _mediator.Send(descendProductStockCommandRequest);
            },
            services: _stockService);
        }
    }
}
