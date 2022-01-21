
using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.CR.Services;
using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.CR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.CR.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.CR.Controllers
{
    [Route("Customers")]
    public class CustomerController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly CustomerService _customerService;

        public CustomerController(IMediator mediator, CustomerService customerService)
        {
            _mediator = mediator;
            _customerService = customerService;
        }

        [HttpGet]
        [Route(nameof(GetCustomers))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetCustomers(CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<GetCustomersQueryResponse>(async () =>
            {
                return await _mediator.Send(new GetCustomersQueryRequest());
            },
            services: _customerService);
        }

        [HttpPost]
        [Route(nameof(CreateCustomer))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommandRequest request, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<CreateCustomerCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _customerService);
        }
    }
}
