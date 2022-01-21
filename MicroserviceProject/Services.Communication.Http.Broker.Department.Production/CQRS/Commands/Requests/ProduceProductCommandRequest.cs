using MediatR;

using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Production.Models;

namespace Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests
{
    public class ProduceProductCommandRequest : IRequest<ProduceProductCommandResponse>
    {
        public ProduceModel Produce { get; set; }
    }
}
