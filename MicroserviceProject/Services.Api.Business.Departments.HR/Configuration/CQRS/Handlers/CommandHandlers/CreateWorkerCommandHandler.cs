using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Api.Business.Departments.HR.Util.Validation.Person.CreateWorker;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateWorkerCommandHandler : IRequestHandler<CreateWorkerCommandRequest, CreateWorkerCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly PersonService _personService;
        private readonly CreateWorkerValidator _createWorkerValidator;

        public CreateWorkerCommandHandler(
            RuntimeHandler runtimeHandler,
            PersonService personService,
            CreateWorkerValidator createWorkerValidator)
        {
            _runtimeHandler = runtimeHandler;
            _personService = personService;
            _createWorkerValidator = createWorkerValidator;
        }

        public async Task<CreateWorkerCommandResponse> Handle(CreateWorkerCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _createWorkerValidator.ValidateAsync(request.Worker, cancellationTokenSource);

            return new CreateWorkerCommandResponse()
            {
                CreatedWorkerId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _personService,
                    nameof(_personService.CreateWorkerAsync),
                    new object[] { request.Worker, cancellationTokenSource })
            };
        }
    }
}
