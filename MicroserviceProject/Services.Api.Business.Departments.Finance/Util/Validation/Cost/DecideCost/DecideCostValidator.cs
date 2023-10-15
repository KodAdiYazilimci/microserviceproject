using Infrastructure.Validation;

using Services.Business.Departments.Finance.Configuration.Validation.Cost.DecideCost;
using Services.Communication.Http.Broker.Department.Finance.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Util.Validation.Cost.DecideCost
{
    /// <summary>
    /// Cpst/DecideCost Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class DecideCostValidator : BaseValidator<DecidedCostModel, DecideCostRule>
    {
        public DecideCostValidator(DecideCostRule validationRule) : base(validationRule)
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
