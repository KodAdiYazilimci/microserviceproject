using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.HR.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetWorkersQueryHandler : IRequestHandler<GetWorkersQueryRequest, GetWorkersQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly PersonService _personService;

        public GetWorkersQueryHandler(
            RuntimeHandler runtimeHandler,
            PersonService personService)
        {
            _runtimeHandler = runtimeHandler;
            _personService = personService;
        }

        public async Task<GetWorkersQueryResponse> Handle(GetWorkersQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetWorkersQueryResponse()
            {
                Workers =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<WorkerModel>>>(
                    _personService,
                    nameof(_personService.GetWorkersAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}
