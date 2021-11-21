using Infrastructure.Security.Authentication.BasicToken.Abstracts;
using Infrastructure.Security.Authentication.BasicToken.Schemes;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Security.Authentication.BasicToken.Handlers
{
    /// <summary>
    /// Kimlik doğrulama denetimi yapacak sınıf
    /// </summary>
    public class TokenHandler : AuthenticationHandler<AuthenticationSchemeOptions>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly IIdentityProvider _identityManager;

        /// <summary>
        /// Kimlik doğrulama denetimi yapacak sınıf
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="urlEncoder"></param>
        /// <param name="systemClock"></param>
        /// <param name="authorizationCommunicator">Kimlik denetimi servisi iletişimcisi</param>
        /// <param name="cacheProvider">Önbellek sağlayıcısı sınıf</param>
        public TokenHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            ISystemClock systemClock,
            IIdentityProvider identityManager) : base(options, loggerFactory, urlEncoder, systemClock)
        {
            _identityManager = identityManager;
        }

        /// <summary>
        /// Kimlik denetimini sağlar
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            if (await _identityManager.GetUserAsync(cancellationTokenSource) != null)
            {
                AuthenticatedUser authenticatedUser = await _identityManager.GetUserAsync(cancellationTokenSource);

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                    claims: authenticatedUser.Claims.Select(x => new Claim(x.Name, x.Value)).ToList(),
                     authenticationType: Default.DefaultScheme);

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Default.DefaultScheme));
            }
            else
            {
                return AuthenticateResult.Fail(_identityManager.AuthenticationFailMessage);
            }
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {

                }

                disposed = true;
            }
        }
    }
}
