﻿using MediatR;

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
    public class GetInventoriesForNewWorkerQueryHandler : IRequestHandler<GetInventoriesForNewWorkerQueryRequest, GetInventoriesForNewWorkerQueryResponse>
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

        public Task<GetInventoriesForNewWorkerQueryResponse> Handle(GetInventoriesForNewWorkerQueryRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            return Task.FromResult(new GetInventoriesForNewWorkerQueryResponse()
            {
                Inventories =
                _runtimeHandler.ExecuteResultMethod<List<InventoryModel>>(
                    _inventoryService,
                    nameof(_inventoryService.GetInventoriesForNewWorker),
                    new object[] { cancellationTokenSource })
            });
        }
    }
}
