using Infrastructure.Validation;

using Services.Api.Authorization.Configuration.Validation.Auth.GetToken;
using Services.Communication.Http.Broker.Authorization.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Authorization.Util.Validation.Auth.GetToken
{
    /// <summary>
    /// Auth/GetToken endpoint için validasyon
    /// </summary>
    public class GetTokenValidator : BaseValidator<CredentialModel, CredentialRule>
    {
        public GetTokenValidator(CredentialRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(CredentialModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
