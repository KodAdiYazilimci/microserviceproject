using Infrastructure.Validation;

using Services.Api.Business.Departments.Buying.Configuration.Validation.Request.ValidateCostInventory;
using Services.Communication.Http.Broker.Department.Buying.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Util.Validation.Request.ValidateCostInventory
{
    /// <summary>
    /// Request/ValidateCostInventory Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class ValidateCostInventoryValidator : BaseValidator<DecidedCostModel, ValidateCostInventoryRule>
    {
        public ValidateCostInventoryValidator(ValidateCostInventoryRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(DecidedCostModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
