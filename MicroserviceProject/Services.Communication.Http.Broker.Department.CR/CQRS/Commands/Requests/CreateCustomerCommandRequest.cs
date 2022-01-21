using MediatR;

using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.CR.Models;

namespace Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Requests
{
    public class CreateCustomerCommandRequest : IRequest<CreateCustomerCommandResponse>
    {
        public CustomerModel Customer { get; set; }
    }
}
