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
    public class GetDecidedCostsQueryHandler : IRequestHandler<GetDecidedCostsQueryRequest, GetDecidedCostsQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly CostService _costService;

        public GetDecidedCostsQueryHandler(
            RuntimeHandler runtimeHandler,
            CostService costService)
        {
            _runtimeHandler = runtimeHandler;
            _costService = costService;
        }

        public async Task<GetDecidedCostsQueryResponse> Handle(GetDecidedCostsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetDecidedCostsQueryResponse()
            {
                DecidedCosts =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<DecidedCostModel>>>(
                    _costService,
                    nameof(_costService.GetProductionRequestsAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}
