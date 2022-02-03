using MediatR;

using Services.Api.Business.Departments.Buying.Services;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.Buying.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetInventoryRequestsQueryHandler : IRequestHandler<GetInventoryRequestsQueryRequest, GetInventoryRequestsQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly RequestService _requestService;

        public GetInventoryRequestsQueryHandler(
            RuntimeHandler runtimeHandler,
            RequestService requestService)
        {
            _runtimeHandler = runtimeHandler;
            _requestService = requestService;
        }

        public async Task<GetInventoryRequestsQueryResponse> Handle(GetInventoryRequestsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetInventoryRequestsQueryResponse()
            {
                InventoryRequests =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<InventoryRequestModel>>>(
                    _requestService,
                    nameof(_requestService.GetInventoryRequestsAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}
