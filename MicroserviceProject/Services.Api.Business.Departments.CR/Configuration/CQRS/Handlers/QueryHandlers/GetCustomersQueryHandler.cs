using MediatR;

using Services.Api.Business.Departments.CR.Services;
using Services.Communication.Http.Broker.Department.CR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.CR.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.CR.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQueryRequest, GetCustomersQueryResponse>
    {
        private readonly CustomerService _customerService;

        public GetCustomersQueryHandler(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<GetCustomersQueryResponse> Handle(GetCustomersQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetCustomersQueryResponse()
            {
                Customers = await _customerService.GetCustomersAsync(new CancellationTokenSource())
            };
        }
    }
}
