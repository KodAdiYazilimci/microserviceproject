using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetInventoriesQueryHandler : IRequestHandler<GetInventoriesQueryRequest, GetInventoriesQueryResponse>
    {
        private readonly InventoryService _inventoryService;

        public GetInventoriesQueryHandler(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<GetInventoriesQueryResponse> Handle(GetInventoriesQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetInventoriesQueryResponse()
            {
                Inventories = await _inventoryService.GetInventoriesAsync(new CancellationTokenSource())
            };
        }
    }
}
