using MediatR;

using Services.Api.Business.Departments.Selling.Services;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.Selling.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Selling.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetSoldsQueryHandler : IRequestHandler<GetSoldsQueryRequest, GetSoldsQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly SellingService _sellingService;

        public GetSoldsQueryHandler(RuntimeHandler runtimeHandler, SellingService sellingService)
        {
            _runtimeHandler = runtimeHandler;
            _sellingService = sellingService;
        }

        public async Task<GetSoldsQueryResponse> Handle(GetSoldsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetSoldsQueryResponse()
            {
                Solds =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<SellModel>>>(
                    _sellingService,
                    nameof(_sellingService.GetSoldsAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}
