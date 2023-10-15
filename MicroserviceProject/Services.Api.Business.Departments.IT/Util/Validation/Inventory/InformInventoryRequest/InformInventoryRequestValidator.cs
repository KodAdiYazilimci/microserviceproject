using Infrastructure.Validation;

using Services.Api.Business.Departments.IT.Configuration.Validation.Inventory.InformInventoryRequest;
using Services.Communication.Http.Broker.Department.IT.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Util.Validation.Inventory.InformInventoryRequest
{
    /// <summary>
    /// Inventory/InformInventoryRequestValidator Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class InformInventoryRequestValidator : BaseValidator<ITInventoryRequestModel, InformInventoryRequestRule>
    {
        public InformInventoryRequestValidator(InformInventoryRequestRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(ITInventoryRequestModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
