using MediatR;

using Services.Business.Departments.Finance.Services;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Finance.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetDecidedCostsQueryHandler : IRequestHandler<GetDecidedCostsQueryRequest, GetDecidedCostsQueryResponse>
    {
        private readonly CostService _costService;

        public GetDecidedCostsQueryHandler(CostService costService)
        {
            _costService = costService;
        }

        public async Task<GetDecidedCostsQueryResponse> Handle(GetDecidedCostsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetDecidedCostsQueryResponse()
            {
                DecidedCosts = await _costService.GetDecidedCostsAsync(new CancellationTokenSource())
            };
        }
    }
}
