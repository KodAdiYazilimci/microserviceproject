using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Api.Business.Departments.HR.Util.Validation.Department.CreateDepartment;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommandRequest, CreateDepartmentCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly DepartmentService _departmentService;

        public CreateDepartmentCommandHandler(
            RuntimeHandler runtimeHandler,
            DepartmentService departmentService)
        {
            _runtimeHandler = runtimeHandler;
            _departmentService = departmentService;
        }

        public async Task<CreateDepartmentCommandResponse> Handle(CreateDepartmentCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateDepartmentValidator.ValidateAsync(request.Department, cancellationTokenSource);

            return new CreateDepartmentCommandResponse()
            {
                CreatedDepartmentId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _departmentService,
                    nameof(_departmentService.CreateDepartmentAsync),
                    new object[] { request.Department, cancellationTokenSource })
            };
        }
    }
}
