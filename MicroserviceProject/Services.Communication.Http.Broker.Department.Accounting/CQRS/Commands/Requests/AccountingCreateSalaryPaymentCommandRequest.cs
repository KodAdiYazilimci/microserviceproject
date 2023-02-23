using MediatR;

using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Accounting.Models;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests
{
    public class AccountingCreateSalaryPaymentCommandRequest : IRequest<AccountingCreateSalaryPaymentCommandResponse>
    {
        public AccountingSalaryPaymentModel SalaryPayment { get; set; }
    }
}
