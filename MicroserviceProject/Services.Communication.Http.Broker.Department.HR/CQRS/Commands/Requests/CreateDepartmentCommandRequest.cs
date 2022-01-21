using MediatR;

using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.HR.Models;

namespace Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests
{
    public class CreateDepartmentCommandRequest : IRequest<CreateDepartmentCommandResponse>
    {
        public DepartmentModel Department { get; set; }
    }
}
