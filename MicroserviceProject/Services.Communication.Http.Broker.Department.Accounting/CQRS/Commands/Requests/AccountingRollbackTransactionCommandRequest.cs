using Infrastructure.Transaction.Recovery;

using MediatR;

using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests
{
    public class AccountingRollbackTransactionCommandRequest : IRequest<AccountingRollbackTransactionCommandResponse>
    {
        public RollbackModel Rollback { get; set; }
    }
}
