using MediatR;

using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.AA.Models;

namespace Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests
{
    public class AAAssignInventoryToWorkerCommandRequest : IRequest<AAAssignInventoryToWorkerCommandResponse>
    {
        public List<AAAssignInventoryToWorkerModel> AssignInventoryToWorkerModels { get; set; }
    }
}
