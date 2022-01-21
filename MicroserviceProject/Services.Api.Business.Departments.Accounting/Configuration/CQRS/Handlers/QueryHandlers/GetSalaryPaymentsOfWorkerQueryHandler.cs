using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetSalaryPaymentsOfWorkerQueryHandler : IRequestHandler<GetSalaryPaymentsOfWorkerQueryRequest, GetSalaryPaymentsOfWorkerQueryResponse>
    {
        private readonly BankService _bankService;

        public GetSalaryPaymentsOfWorkerQueryHandler(BankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<GetSalaryPaymentsOfWorkerQueryResponse> Handle(GetSalaryPaymentsOfWorkerQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetSalaryPaymentsOfWorkerQueryResponse()
            {
                SalaryPayments = await _bankService.GetSalaryPaymentsOfWorkerAsync(request.WorkerId, new CancellationTokenSource())
            };
        }
    }
}
