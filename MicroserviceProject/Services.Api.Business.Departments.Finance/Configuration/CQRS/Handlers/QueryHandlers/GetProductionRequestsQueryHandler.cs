using MediatR;

using Services.Business.Departments.Finance.Services;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Finance.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetProductionRequestsQueryHandler : IRequestHandler<GetProductionRequestsQueryRequest, GetProductionRequestsQueryResponse>
    {
        private readonly ProductionRequestService _productionRequestService;

        public GetProductionRequestsQueryHandler(ProductionRequestService productionRequestService)
        {
            _productionRequestService = productionRequestService;
        }

        public async Task<GetProductionRequestsQueryResponse> Handle(GetProductionRequestsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetProductionRequestsQueryResponse()
            {
                ProductionRequests =
                await _productionRequestService.GetProductionRequestsAsync(new CancellationTokenSource())
            };
        }
    }
}
