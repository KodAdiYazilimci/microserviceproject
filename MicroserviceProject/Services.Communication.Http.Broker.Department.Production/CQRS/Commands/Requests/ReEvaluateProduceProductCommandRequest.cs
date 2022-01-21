using MediatR;

using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Responses;

namespace Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests
{
    public class ReEvaluateProduceProductCommandRequest : IRequest<ReEvaluateProduceProductCommandResponse>
    {
        public int ReferenceNumber { get; set; }
    }
}
