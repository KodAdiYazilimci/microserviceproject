using Communication.Http.Department.CR.Models;

using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Business.Departments.CR.Services;
using Services.Business.Departments.CR.Util.Validation.Customer.CreateCustomer;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.CR.Controllers
{
    [Authorize]
    [Route("Customers")]
    public class CustomerController : BaseController
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route(nameof(GetCustomers))]
        public async Task<IActionResult> GetCustomers(CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<CustomerModel>>(async () =>
            {
                return await _customerService.GetCustomersAsync(cancellationTokenSource);
            },
            services: _customerService);
        }

        [HttpPost]
        [Route(nameof(CreateCustomer))]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerModel customerModel, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateCustomerValidator.ValidateAsync(customerModel, cancellationTokenSource);

                return await _customerService.CreateCustomerAsync(customerModel, cancellationTokenSource);
            },
            services: _customerService);
        }
    }
}
