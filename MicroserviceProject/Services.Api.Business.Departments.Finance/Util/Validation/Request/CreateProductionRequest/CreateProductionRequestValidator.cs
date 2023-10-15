using Infrastructure.Validation;

using Services.Business.Departments.Finance.Configuration.Validation.Request.CreateProductionRequest;
using Services.Communication.Http.Broker.Department.Finance.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Util.Validation.Request.CreateProductionRequest
{
    /// <summary>
    /// Request/CreateProductionRequest Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateProductionRequestValidator : BaseValidator<ProductionRequestModel, CreateProductionRequestRule>
    {
        public CreateProductionRequestValidator(CreateProductionRequestRule validationRule) : base(validationRule)
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
