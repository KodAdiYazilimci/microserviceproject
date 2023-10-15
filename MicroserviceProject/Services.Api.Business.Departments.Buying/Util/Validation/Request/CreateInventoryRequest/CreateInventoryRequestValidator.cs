using Infrastructure.Validation;

using Services.Api.Business.Departments.Buying.Configuration.Validation.Request.CreateInventoryRequest;
using Services.Communication.Http.Broker.Department.Buying.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Util.Validation.Request.CreateInventoryRequest
{
    /// <summary>
    /// Request/CreateInventoryRequest Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateInventoryRequestValidator : BaseValidator<InventoryRequestModel, CreateInventoryRequestRule>
    {
        public CreateInventoryRequestValidator(CreateInventoryRequestRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(InventoryRequestModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
