using MediatR;

using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.HR.Models;

namespace Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests
{
    public class CreatePersonCommandRequest : IRequest<CreatePersonCommandResponse>
    {
        public PersonModel Person { get; set; }
    }
}
