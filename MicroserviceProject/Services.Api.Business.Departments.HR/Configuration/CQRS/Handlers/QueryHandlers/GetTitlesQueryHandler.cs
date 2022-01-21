using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetTitlesQueryHandler : IRequestHandler<GetTitlesQueryRequest, GetTitlesQueryResponse>
    {
        private readonly PersonService _personService;

        public GetTitlesQueryHandler(PersonService personService)
        {
            _personService = personService;
        }

        public async Task<GetTitlesQueryResponse> Handle(GetTitlesQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetTitlesQueryResponse()
            {
                Titles = await _personService.GetTitlesAsync(new CancellationTokenSource())
            };
        }
    }
}
