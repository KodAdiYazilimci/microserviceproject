using Infrastructure.Transaction.Recovery;

using MediatR;

using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;

namespace Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests
{
    public class AARollbackTransactionCommandRequest : IRequest<AARollbackTransactionCommandResponse>
    {
        public RollbackModel Rollback { get; set; }
    }
}
