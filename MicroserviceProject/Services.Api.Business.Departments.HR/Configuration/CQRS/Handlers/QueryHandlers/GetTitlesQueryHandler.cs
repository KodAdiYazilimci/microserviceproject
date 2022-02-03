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
    public class GetTitlesQueryHandler : IRequestHandler<GetTitlesQueryRequest, GetTitlesQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly PersonService _personService;

        public GetTitlesQueryHandler(
            RuntimeHandler runtimeHandler,
            PersonService personService)
        {
            _runtimeHandler = runtimeHandler;
            _personService = personService;
        }

        public async Task<GetTitlesQueryResponse> Handle(GetTitlesQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetTitlesQueryResponse()
            {
                Titles =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<TitleModel>>>(
                    _personService,
                    nameof(_personService.GetTitlesAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}
