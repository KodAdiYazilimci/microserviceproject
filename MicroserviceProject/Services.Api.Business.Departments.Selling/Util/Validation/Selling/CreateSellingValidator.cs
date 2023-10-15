using Infrastructure.Validation;

using Services.Api.Business.Departments.Selling.Configuration.Validation.Selling;
using Services.Communication.Http.Broker.Department.Selling.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Selling.Util.Validation.Selling
{
    /// <summary>
    /// Selling/CreateSelling Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateSellingValidator : BaseValidator<SellModel, CreateSellingRule>
    {
        public CreateSellingValidator(CreateSellingRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(SellModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
