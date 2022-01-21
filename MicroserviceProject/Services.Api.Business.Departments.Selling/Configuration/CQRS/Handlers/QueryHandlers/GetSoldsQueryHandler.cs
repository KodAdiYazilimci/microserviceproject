using MediatR;

using Services.Api.Business.Departments.Selling.Services;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Selling.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetSoldsQueryHandler : IRequestHandler<GetSoldsQueryRequest, GetSoldsQueryResponse>
    {
        private readonly SellingService _sellingService;

        public GetSoldsQueryHandler(SellingService sellingService)
        {
            _sellingService = sellingService;
        }

        public async Task<GetSoldsQueryResponse> Handle(GetSoldsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetSoldsQueryResponse()
            {
                Solds = await _sellingService.GetSoldsAsync(new CancellationTokenSource())
            };
        }
    }
}
