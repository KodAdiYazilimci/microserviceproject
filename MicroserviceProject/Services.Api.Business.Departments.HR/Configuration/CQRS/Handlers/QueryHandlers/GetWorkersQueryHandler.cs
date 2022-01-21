using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetWorkersQueryHandler : IRequestHandler<GetWorkersQueryRequest, GetWorkersQueryResponse>
    {
        private readonly PersonService _personService;

        public GetWorkersQueryHandler(PersonService personService)
        {
            _personService = personService;
        }

        public async Task<GetWorkersQueryResponse> Handle(GetWorkersQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetWorkersQueryResponse()
            {
                Workers = await _personService.GetWorkersAsync(new CancellationTokenSource())
            };
        }
    }
}
