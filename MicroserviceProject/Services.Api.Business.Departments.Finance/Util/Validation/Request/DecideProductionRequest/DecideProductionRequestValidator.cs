using Infrastructure.Validation;

using Services.Business.Departments.Finance.Configuration.Validation.Request.DecideProductionRequest;
using Services.Communication.Http.Broker.Department.Finance.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Util.Validation.Request.CreateProductionRequest
{
    /// <summary>
    /// Request/DecideProductionRequest Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class DecideProductionRequestValidator : BaseValidator<ProductionRequestModel, DecideProductionRequestRule>
    {
        public DecideProductionRequestValidator(DecideProductionRequestRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(ProductionRequestModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
