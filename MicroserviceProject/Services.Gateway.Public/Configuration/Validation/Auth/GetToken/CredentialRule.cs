using FluentValidation;

using Infrastructure.Security.Model;

namespace Services.Gateway.Public.Validation.Auth.GetToken
{
    /// <summary>
    /// Auth/GetToken endpoint için validasyon kuralı
    /// </summary>
    public class CredentialRule : AbstractValidator<Credential>
    {
        /// <summary>
        /// Auth/GetToken endpoint için validasyon kuralı
        /// </summary>
        public CredentialRule()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta boş geçilemez");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Geçersiz e-posta adresi");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola boş geçilemez");
        }
    }
}
