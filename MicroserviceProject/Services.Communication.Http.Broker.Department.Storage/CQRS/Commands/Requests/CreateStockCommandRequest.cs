using MediatR;

using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Storage.Models;

namespace Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Requests
{
    public class CreateStockCommandRequest : IRequest<CreateStockCommandResponse>
    {
        public StockModel Stock { get; set; }
    }
}
