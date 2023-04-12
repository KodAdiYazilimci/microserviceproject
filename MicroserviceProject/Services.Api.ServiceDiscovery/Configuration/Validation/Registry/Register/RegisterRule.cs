using FluentValidation;

using Services.Api.ServiceDiscovery.Dto;

namespace Services.Api.ServiceDiscovery.Configuration.Validation.Registry.Register
{
    public class RegisterRule : AbstractValidator<ServiceDto>
    {
        public RegisterRule()
        {
            RuleFor(x => x.ServiceName).NotEmpty().WithMessage("Servis adı boş geçilemez");
            RuleFor(x => x.Port).GreaterThan(0).WithMessage("Geçersiz port");
            RuleFor(x => x.Protocol).NotEmpty().WithMessage("Protokol boş geçilemez");
        }
    }
}
