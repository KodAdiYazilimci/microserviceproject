using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQueryRequest, GetCurrenciesQueryResponse>
    {
        private readonly BankService _bankService;

        public GetCurrenciesQueryHandler(BankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<GetCurrenciesQueryResponse> Handle(GetCurrenciesQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetCurrenciesQueryResponse()
            {
                Currencies = await _bankService.GetCurrenciesAsync(new CancellationTokenSource())
            };
        }
    }
}
