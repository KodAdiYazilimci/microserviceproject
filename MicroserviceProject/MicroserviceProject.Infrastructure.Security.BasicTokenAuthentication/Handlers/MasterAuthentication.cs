using MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Persistence;
using MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Schemes;
using MicroserviceProject.Model.Security;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Handlers
{
    /// <summary>
    /// Kimlik doğrulama denetimi yapacak sınıf
    /// </summary>
    public class MasterAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Kimlik doğrulama denetimi yapacak sınıf
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="urlEncoder"></param>
        /// <param name="systemClock"></param>
        public MasterAuthentication(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            ISystemClock systemClock) : base(options, loggerFactory, urlEncoder, systemClock)
        {
        }

        /// <summary>
        /// Kimlik denetimini sağlar
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            if (await GetUserAsync(cancellationTokenSource.Token) != null)
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(AuthenticationPersistence.GetClaims(await GetUserAsync(cancellationTokenSource.Token)), Default.DefaultScheme);

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Default.DefaultScheme));
            }
            else
            {
                return AuthenticateResult.Fail(new Exception("Lütfen tekrar oturum açın!"));
            }
        }

        /// <summary>
        /// Oturumda bulunan kullanıcı
        /// </summary>
        protected async Task<User> GetUserAsync(CancellationToken cancellationToken)
        {
            if (Request.Headers.TryGetValue("token", out StringValues headerToken) && headerToken.Any(x => x.Length > 0))
            {
                return new User();
            }

            //throw new SessionExpiredException("Lütfen tekrar oturum açın!");
            return null;
        }
    }
}
