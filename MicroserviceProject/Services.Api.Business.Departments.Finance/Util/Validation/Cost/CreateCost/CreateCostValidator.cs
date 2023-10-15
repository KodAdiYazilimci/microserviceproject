using Infrastructure.Validation;

using Services.Business.Departments.Finance.Configuration.Validation.Cost.CreateCost;
using Services.Communication.Http.Broker.Department.Finance.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Util.Validation.Cost.CreateCost
{
    /// <summary>
    /// Cpst/CreateCost Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateCostValidator : BaseValidator<DecidedCostModel, CreateCostRule>
    {
        public CreateCostValidator(CreateCostRule validationRule) : base(validationRule)
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
