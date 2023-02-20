using FluentValidation;

using Infrastructure.Security.Model;

namespace Services.Api.Gateway.Validation.Auth.GetToken
{
    /// <summary>
    /// Auth/GetToken endpoint için validasyon kuralı
    /// </summary>
    public class CredentialRule : AbstractValidator<AuthenticationCredential>
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
