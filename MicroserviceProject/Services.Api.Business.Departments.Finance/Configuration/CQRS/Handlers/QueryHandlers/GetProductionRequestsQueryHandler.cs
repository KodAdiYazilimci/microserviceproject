using MediatR;

using Services.Business.Departments.Finance.Services;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.Finance.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Finance.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetProductionRequestsQueryHandler : IRequestHandler<GetProductionRequestsQueryRequest, GetProductionRequestsQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly ProductionRequestService _productionRequestService;

        public GetProductionRequestsQueryHandler(
            RuntimeHandler runtimeHandler,
            ProductionRequestService productionRequestService)
        {
            _runtimeHandler = runtimeHandler;
            _productionRequestService = productionRequestService;
        }

        public async Task<GetProductionRequestsQueryResponse> Handle(GetProductionRequestsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetProductionRequestsQueryResponse()
            {
                ProductionRequests =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<ProductionRequestModel>>>(
                    _productionRequestService,
                    nameof(_productionRequestService.GetProductionRequestsAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}
