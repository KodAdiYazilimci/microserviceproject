using MediatR;

using Services.Api.Business.Departments.Production.Services;
using Services.Communication.Http.Broker.Department.Production.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Production.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.Production.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQueryRequest, GetProductsQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly ProductService _productService;

        public GetProductsQueryHandler(
            RuntimeHandler runtimeHandler,
            ProductService productService)
        {
            _runtimeHandler = runtimeHandler;
            _productService = productService;
        }

        public async Task<GetProductsQueryResponse> Handle(GetProductsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetProductsQueryResponse()
            {
                Products =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<ProductModel>>>(
                    _productService,
                    nameof(_productService.GetProductsAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}
