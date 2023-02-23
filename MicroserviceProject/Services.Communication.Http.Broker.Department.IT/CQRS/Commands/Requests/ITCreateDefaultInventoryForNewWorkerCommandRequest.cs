using MediatR;

using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.IT.Models;

namespace Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests
{
    public class ITCreateDefaultInventoryForNewWorkerCommandRequest : IRequest<ITCreateDefaultInventoryForNewWorkerCommandResponse>
    {
        public ITDefaultInventoryForNewWorkerModel DefaultInventoryForNewWorkerModel { get; set; }
    }
}
