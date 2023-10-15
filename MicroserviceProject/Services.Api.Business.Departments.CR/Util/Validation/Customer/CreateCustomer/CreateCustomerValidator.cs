using Infrastructure.Validation;

using Services.Api.Business.Departments.CR.Configuration.Validation.Customer.CreateCustomer;
using Services.Communication.Http.Broker.Department.CR.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.CR.Util.Validation.Customer.CreateCustomer
{
    /// <summary>
    /// Customer/CreateCustomer Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateCustomerValidator : BaseValidator<CustomerModel, CreateCustomerRule>
    {
        public CreateCustomerValidator(CreateCustomerRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(CustomerModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
