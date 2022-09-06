using Infrastructure.Security.Authentication.Cookie.Abstract;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Security.Authentication.Cookie.Handlers
{
    public class CookieHandler
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly IIdentityProvider _identityProvider;

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        /// <param name="authorizationCommunicator">Kimlik denetimi servisi iletişimcisi</param>
        public CookieHandler(
            IHttpContextAccessor httpContextAccessor,
            IIdentityProvider identityProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityProvider = identityProvider;
        }

        /// <summary>
        /// Kullanıcı bilgilerine göre kullanıcıyı oturuma dahil eder
        /// </summary>
        /// <param name="httpContext">HttpContext nesnesi</param>
        /// <param name="credential">Kullanıcı bilgileri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<bool> LoginAsync(AuthenticationCredential credential, CancellationTokenSource cancellationTokenSource)
        {
            AuthenticatedUser authenticatedUser = await _identityProvider.GetUserAsync(credential, cancellationTokenSource);

            if (authenticatedUser != null)
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                    claims: authenticatedUser.Claims.Select(x => new Claim(x.Name, x.Value)).ToList(),
                    authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    new AuthenticationProperties()
                    {
                        ExpiresUtc = new DateTimeOffset(authenticatedUser.Token.ValidTo, TimeSpan.Zero)
                    });

                _identityProvider.SetToCache(authenticatedUser);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Token bilgisine göre kullanıcıyı oturuma dahil eder
        /// </summary>
        /// <param name="httpContext">HttpContext nesnesi</param>
        /// <param name="token">Token bilgisi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<bool> LoginAsync(string token, CancellationTokenSource cancellationTokenSource)
        {
            AuthenticatedUser authenticatedUser = await _identityProvider.GetUserAsync(token, cancellationTokenSource);

            if (authenticatedUser != null)
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                    claims: authenticatedUser.Claims.Select(x => new Claim(x.Name, x.Value)).ToList(),
                    authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    new AuthenticationProperties()
                    {
                        ExpiresUtc = new DateTimeOffset(authenticatedUser.Token.ValidTo, TimeSpan.Zero)
                    });

                _identityProvider.SetToCache(authenticatedUser);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Kullanıcı oturumunu sonlandırır
        /// </summary>
        /// <returns></returns>
        public async Task LogOutAsync()
        {
            Claim claim = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.UserData).FirstOrDefault();

            if (claim != null)
            {
                AuthenticatedUser authenticatedUser = _identityProvider.GetUserFromCache(claim.Value);

                if (authenticatedUser != null)
                {
                    _identityProvider.RemoveUserFromCache(authenticatedUser.Token.TokenKey);
                }
            }

            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Oturum açmış kullanıcıyı verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<AuthenticatedUser> GetLoggedInUserAsyc(CancellationTokenSource cancellationTokenSource)
        {
            Claim claim = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.UserData).FirstOrDefault();

            if (claim != null)
            {
                return await _identityProvider.GetUserAsync(claim.Value, cancellationTokenSource);
            }

            return null;
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
