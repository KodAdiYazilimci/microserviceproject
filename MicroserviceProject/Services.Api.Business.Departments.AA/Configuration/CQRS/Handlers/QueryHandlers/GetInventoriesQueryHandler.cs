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
    public class GetInventoriesQueryHandler : IRequestHandler<AAGetInventoriesQueryRequest, AAGetInventoriesQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly InventoryService _inventoryService;

        public GetInventoriesQueryHandler(
            RuntimeHandler runtimeHandler,
            InventoryService inventoryService)
        {
            _runtimeHandler = runtimeHandler;
            _inventoryService = inventoryService;
        }

        public async Task<AAGetInventoriesQueryResponse> Handle(AAGetInventoriesQueryRequest request, CancellationToken cancellationToken)
        {
            return new AAGetInventoriesQueryResponse()
            {
                Inventories =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<AAInventoryModel>>>(
                    _inventoryService,
                    nameof(_inventoryService.GetInventoriesAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}
