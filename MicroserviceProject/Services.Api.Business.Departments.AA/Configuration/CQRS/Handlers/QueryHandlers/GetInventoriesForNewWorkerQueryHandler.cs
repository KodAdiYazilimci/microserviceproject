using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.AA.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetInventoriesForNewWorkerQueryHandler : IRequestHandler<AAGetInventoriesForNewWorkerQueryRequest, AAGetInventoriesForNewWorkerQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly InventoryService _inventoryService;

        public GetInventoriesForNewWorkerQueryHandler(
            RuntimeHandler runtimeHandler,
            InventoryService inventoryService)
        {
            _runtimeHandler = runtimeHandler;
            _inventoryService = inventoryService;
        }

        public Task<AAGetInventoriesForNewWorkerQueryResponse> Handle(AAGetInventoriesForNewWorkerQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new AAGetInventoriesForNewWorkerQueryResponse()
            {
                Inventories =
                _runtimeHandler.ExecuteResultMethod<List<AADefaultInventoryForNewWorkerModel>>(
                    _inventoryService,
                    nameof(_inventoryService.GetInventoriesForNewWorker),
                    new object[] { new CancellationTokenSource() })
            });
        }
    }
}
