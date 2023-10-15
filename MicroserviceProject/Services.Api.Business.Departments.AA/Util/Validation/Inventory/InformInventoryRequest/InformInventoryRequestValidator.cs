using Infrastructure.Validation;

using Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.InformInventoryRequest;
using Services.Communication.Http.Broker.Department.AA.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Util.Validation.Inventory.InformInventoryRequest
{
    /// <summary>
    /// Inventory/InformInventoryRequestValidator Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class InformInventoryRequestValidator : BaseValidator<AAInventoryRequestModel, InformInventoryRequestRule>
    {
        public InformInventoryRequestValidator(InformInventoryRequestRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(AAInventoryRequestModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
