using Infrastructure.Transaction.Recovery;

using MediatR;

using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;

namespace Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests
{
    public class RollbackTransactionCommandRequest : IRequest<RollbackTransactionCommandResponse>
    {
        public RollbackModel Rollback { get; set; }
    }
}
