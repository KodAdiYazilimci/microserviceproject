using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetInventoriesForNewWorkerQueryHandler : IRequestHandler<GetInventoriesForNewWorkerQueryRequest, GetInventoriesForNewWorkerQueryResponse>
    {
        private readonly InventoryService _inventoryService;

        public GetInventoriesForNewWorkerQueryHandler(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public Task<GetInventoriesForNewWorkerQueryResponse> Handle(GetInventoriesForNewWorkerQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetInventoriesForNewWorkerQueryResponse()
            {
                Inventories = _inventoryService.GetInventoriesForNewWorker(new CancellationTokenSource())
            });
        }
    }
}
