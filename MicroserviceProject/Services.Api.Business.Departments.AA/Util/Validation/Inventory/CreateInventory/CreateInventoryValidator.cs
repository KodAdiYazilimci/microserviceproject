using Infrastructure.Validation;

using Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.CreateInventory;
using Services.Communication.Http.Broker.Department.AA.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateInventory
{
    /// <summary>
    /// Inventory/CreateInventory Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateInventoryValidator : BaseValidator<AAInventoryModel, CreateInventoryRule>
    {
        public CreateInventoryValidator(CreateInventoryRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(AAInventoryModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
