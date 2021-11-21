using Infrastructure.Security.Authentication.SignalR.Abstract;
using Infrastructure.Security.Authentication.SignalR.Requirements;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Security.Authentication.SignalR.Handlers
{
    /// <summary>
    /// Varsayılan kimlik denetimi yapan sınıf
    /// </summary>
    public class SignalRHandler : AuthorizationHandler<DefaultAuthorizationRequirement>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Http üst öğelerine erişim sağlayacak nesne
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IIdentityProvider _identityProvider;

        /// <summary>
        /// Varsayılan kimlik denetimi yapan sınıf
        /// </summary>
        /// <param name="httpContextAccessor">Http üst öğelerine erişim sağlayacak nesne</param>
        /// <param name="cacheProvider">Oturum bilgilerinin saklanacağı önbellek nesnesi</param>
        /// <param name="authorizationCommunicator">Kimlik denetimi servisi iletişimcisi</param>
        public SignalRHandler(
            IHttpContextAccessor httpContextAccessor,
            IIdentityProvider identityProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityProvider = identityProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultAuthorizationRequirement requirement)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            AuthenticatedUser authenticatedUser = _identityProvider.GetUserFromCache(token);

            if (authenticatedUser != null)
            {
                context.Succeed(requirement);
            }
            else
            {
                if (!string.IsNullOrEmpty(token))
                {
                    authenticatedUser = await _identityProvider.GetUserAsync(token, cancellationTokenSource);

                    if (authenticatedUser != null)
                    {
                        _identityProvider.SetToCache(authenticatedUser);

                        context.Succeed(requirement);
                    }
                    else
                        context.Fail();
                }
                else
                {
                    context.Fail();
                }
            }
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

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
