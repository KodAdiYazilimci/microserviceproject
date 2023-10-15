using Infrastructure.Validation;

using Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Communication.Http.Broker.Department.AA.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker
{
    /// <summary>
    /// Inventory/CreateDefaultInventoryForNewWorker Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateDefaultInventoryForNewWorkerValidator : BaseValidator<AADefaultInventoryForNewWorkerModel, CreateDefaultInventoryForNewWorkerRule>
    {
        public CreateDefaultInventoryForNewWorkerValidator(CreateDefaultInventoryForNewWorkerRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(AADefaultInventoryForNewWorkerModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
