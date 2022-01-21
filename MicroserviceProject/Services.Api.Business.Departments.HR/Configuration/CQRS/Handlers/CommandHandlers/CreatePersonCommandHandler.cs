using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Api.Business.Departments.HR.Util.Validation.Person.CreatePerson;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommandRequest, CreatePersonCommandResponse>
    {
        private readonly PersonService _personService;

        public CreatePersonCommandHandler(PersonService personService)
        {
            _personService = personService;
        }

        public async Task<CreatePersonCommandResponse> Handle(CreatePersonCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreatePersonValidator.ValidateAsync(request.Person, cancellationTokenSource);

            return new CreatePersonCommandResponse()
            {
                CreatedPersonId = await _personService.CreatePersonAsync(request.Person, cancellationTokenSource)
            };
        }
    }
}
