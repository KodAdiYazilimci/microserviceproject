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
    public class GetSalaryPaymentsOfWorkerQueryHandler : IRequestHandler<GetSalaryPaymentsOfWorkerQueryRequest, GetSalaryPaymentsOfWorkerQueryResponse>
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

        public async Task<GetSalaryPaymentsOfWorkerQueryResponse> Handle(GetSalaryPaymentsOfWorkerQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetSalaryPaymentsOfWorkerQueryResponse()
            {
                SalaryPayments =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<SalaryPaymentModel>>>(
                    _bankService,
                    nameof(_bankService.GetSalaryPaymentsOfWorkerAsync),
                    new object[] { request.WorkerId, new CancellationTokenSource() })
            };
        }
    }
}
