using Infrastructure.Validation;

using Services.Api.Business.Departments.Accounting.Configuration.Validation.BankAccounts.CreateBankAccount;
using Services.Communication.Http.Broker.Department.Accounting.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment
{
    /// <summary>
    /// BankAccounts/CreateSalaryPayment Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateSalaryPaymentValidator : BaseValidator<AccountingSalaryPaymentModel, CreateSalaryPaymentRule>
    {
        public CreateSalaryPaymentValidator(CreateSalaryPaymentRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(AccountingSalaryPaymentModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
