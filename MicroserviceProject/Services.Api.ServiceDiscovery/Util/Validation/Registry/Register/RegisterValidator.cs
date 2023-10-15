using Infrastructure.Validation;

using Services.Api.ServiceDiscovery.Configuration.Validation.Registry.Register;
using Services.Api.ServiceDiscovery.Dto;

namespace Services.Api.ServiceDiscovery.Util.Validation.Registry.Register
{
    public class RegisterValidator : BaseValidator<ServiceDto, RegisterRule>
    {
        public RegisterValidator(RegisterRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(ServiceDto entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
