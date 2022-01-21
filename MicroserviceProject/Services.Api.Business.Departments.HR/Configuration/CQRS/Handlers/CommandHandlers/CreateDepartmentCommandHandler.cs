using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Api.Business.Departments.HR.Util.Validation.Department.CreateDepartment;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommandRequest, CreateDepartmentCommandResponse>
    {
        private readonly DepartmentService _departmentService;

        public CreateDepartmentCommandHandler(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<CreateDepartmentCommandResponse> Handle(CreateDepartmentCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateDepartmentValidator.ValidateAsync(request.Department, cancellationTokenSource);

            return new CreateDepartmentCommandResponse()
            {
                CreatedDepartmentId = await _departmentService.CreateDepartmentAsync(request.Department, cancellationTokenSource)
            };
        }
    }
}
