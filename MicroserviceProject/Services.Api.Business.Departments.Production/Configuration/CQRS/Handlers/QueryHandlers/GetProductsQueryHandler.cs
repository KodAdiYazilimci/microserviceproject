using MediatR;

using Services.Api.Business.Departments.Production.Services;
using Services.Communication.Http.Broker.Department.Production.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Production.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQueryRequest, GetProductsQueryResponse>
    {
        private readonly ProductService _productService;

        public GetProductsQueryHandler(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductsQueryResponse> Handle(GetProductsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetProductsQueryResponse()
            {
                Products = await _productService.GetProductsAsync(new CancellationTokenSource())
            };
        }
    }
}
