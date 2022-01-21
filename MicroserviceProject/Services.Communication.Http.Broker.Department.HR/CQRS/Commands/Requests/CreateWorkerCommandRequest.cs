using MediatR;

using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.HR.Models;

namespace Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests
{
    public class CreateWorkerCommandRequest : IRequest<CreateWorkerCommandResponse>
    {
        public WorkerModel Worker { get; set; }
    }
}
