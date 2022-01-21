using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetPeopleQueryHandler : IRequestHandler<GetPeopleQueryRequest, GetPeopleQueryResponse>
    {
        private readonly PersonService _personService;

        public GetPeopleQueryHandler(PersonService personService)
        {
            _personService = personService;
        }

        public async Task<GetPeopleQueryResponse> Handle(GetPeopleQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetPeopleQueryResponse()
            {
                People = await _personService.GetPeopleAsync(new CancellationTokenSource())
            };
        }
    }
}
