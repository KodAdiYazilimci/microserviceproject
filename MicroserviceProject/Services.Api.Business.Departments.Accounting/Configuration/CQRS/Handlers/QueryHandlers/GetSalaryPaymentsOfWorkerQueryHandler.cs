using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.Accounting.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetSalaryPaymentsOfWorkerQueryHandler : IRequestHandler<AccountingGetSalaryPaymentsOfWorkerQueryRequest, AccountingGetSalaryPaymentsOfWorkerQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly BankService _bankService;

        public GetSalaryPaymentsOfWorkerQueryHandler(
            RuntimeHandler runtimeHandler,
            BankService bankService)
        {
            _runtimeHandler = runtimeHandler;
            _bankService = bankService;
        }

        public async Task<AccountingGetSalaryPaymentsOfWorkerQueryResponse> Handle(AccountingGetSalaryPaymentsOfWorkerQueryRequest request, CancellationToken cancellationToken)
        {
            return new AccountingGetSalaryPaymentsOfWorkerQueryResponse()
            {
                SalaryPayments =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<AccountingSalaryPaymentModel>>>(
                    _bankService,
                    nameof(_bankService.GetSalaryPaymentsOfWorkerAsync),
                    new object[] { request.WorkerId, new CancellationTokenSource() })
            };
        }
    }
}
