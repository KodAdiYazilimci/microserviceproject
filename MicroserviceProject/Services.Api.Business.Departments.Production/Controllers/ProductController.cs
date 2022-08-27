using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Production.Services;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Production.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Production.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Controllers
{
    [Route("Product")]
    public class ProductController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ProductService _productService;

        public ProductController(
            IMediator mediator,
            ProductService productService)
        {
            _mediator = mediator;
            _productService = productService;
        }

        [HttpGet]
        [Route(nameof(GetProducts))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetProducts()
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                GetProductsQueryResponse mediatorResult = await _mediator.Send(new GetProductsQueryRequest());

                return mediatorResult.Products;
            },
            services: _productService);
        }

        [HttpPost]
        [Route(nameof(CreateProduct))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommandRequest request, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(request);
            },
            services: _productService);
        }
    }
}
