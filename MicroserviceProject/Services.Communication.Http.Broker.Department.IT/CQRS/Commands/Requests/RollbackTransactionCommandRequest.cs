using Infrastructure.Transaction.Recovery;

using MediatR;

using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses;

namespace Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests
{
    public class RollbackTransactionCommandRequest : IRequest<RollbackTransactionCommandResponse>
    {
        public RollbackModel Rollback { get; set; }
    }
}
