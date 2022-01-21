using MediatR;

using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Finance.Models;

namespace Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests
{
    public class CreateProductionRequestCommandRequest : IRequest<CreateProductionRequestCommandResponse>
    {
        public ProductionRequestModel ProductionRequest { get; set; }
    }
}
