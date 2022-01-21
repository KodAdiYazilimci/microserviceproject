using MediatR;

using Services.Api.Business.Departments.IT.Services;
using Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Configuration.CQRS.Handlers.QueryHandlers
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
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            return Task.FromResult(new GetInventoriesForNewWorkerQueryResponse()
            {
                Inventories = _inventoryService.GetInventoriesForNewWorker(cancellationTokenSource)
            });
        }
    }
}
