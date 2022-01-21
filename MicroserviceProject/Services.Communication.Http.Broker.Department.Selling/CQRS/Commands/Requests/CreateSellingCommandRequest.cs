using MediatR;

using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Selling.Models;

namespace Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Requests
{
    public class CreateSellingCommandRequest : IRequest<CreateSellingCommandResponse>
    {
        public SellModel Selling { get; set; }
    }
}
