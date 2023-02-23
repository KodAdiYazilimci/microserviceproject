using MediatR;

using Services.Api.Business.Departments.IT.Services;
using Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.IT.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetInventoriesQueryHandler : IRequestHandler<ITGetInventoriesQueryRequest, ITGetInventoriesQueryResponse>
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

        public async Task<ITGetInventoriesQueryResponse> Handle(ITGetInventoriesQueryRequest request, CancellationToken cancellationToken)
        {
            return new ITGetInventoriesQueryResponse()
            {
                Inventories =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<ITInventoryModel>>>(
                    _inventoryService,
                    nameof(_inventoryService.GetInventoriesAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}
