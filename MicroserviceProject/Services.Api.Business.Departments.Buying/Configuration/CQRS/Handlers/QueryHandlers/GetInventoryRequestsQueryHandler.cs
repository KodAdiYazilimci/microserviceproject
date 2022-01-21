using MediatR;

using Services.Api.Business.Departments.Buying.Services;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetInventoryRequestsQueryHandler : IRequestHandler<GetInventoryRequestsQueryRequest, GetInventoryRequestsQueryResponse>
    {
        private readonly RequestService _requestService;

        public GetInventoryRequestsQueryHandler(RequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<GetInventoryRequestsQueryResponse> Handle(GetInventoryRequestsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetInventoryRequestsQueryResponse()
            {
                InventoryRequests = await _requestService.GetInventoryRequestsAsync(new CancellationTokenSource())
            };
        }
    }
}
