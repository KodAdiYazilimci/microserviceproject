using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Api.Business.Departments.HR.Util.Validation.Person.CreateWorker;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateWorkerCommandHandler : IRequestHandler<CreateWorkerCommandRequest, CreateWorkerCommandResponse>
    {
        private readonly PersonService _personService;

        public CreateWorkerCommandHandler(PersonService personService)
        {
            _personService = personService;
        }

        public async Task<CreateWorkerCommandResponse> Handle(CreateWorkerCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateWorkerValidator.ValidateAsync(request.Worker, cancellationTokenSource);

            return new CreateWorkerCommandResponse()
            {
                CreatedWorkerId = await _personService.CreateWorkerAsync(request.Worker, cancellationTokenSource)
            };
        }
    }
}
