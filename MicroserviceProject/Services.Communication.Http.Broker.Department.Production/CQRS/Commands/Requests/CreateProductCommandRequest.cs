using MediatR;

using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Production.Models;

namespace Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests
{
    public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
    {
        public ProductModel Product { get; set; }
    }
}
