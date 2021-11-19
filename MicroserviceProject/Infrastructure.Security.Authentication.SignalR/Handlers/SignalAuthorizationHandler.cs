using Communication.Http.Authorization;

using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.SignalR.Requirements;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Security.Authentication.SignalR.Handlers
{
    /// <summary>
    /// Varsayılan kimlik denetimi yapan sınıf
    /// </summary>
    public class SignalAuthorizationHandler : AuthorizationHandler<DefaultAuthorizationRequirement>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Önbellekte tutulacak token bazlı kullanıcı oturumları için önbellek anahtarı
        /// </summary>
        private const string CACHEDTOKENBASEDSESSIONS = "CACHED_TOKENBASED_SESSIONS";

        /// <summary>
        /// Http üst öğelerine erişim sağlayacak nesne
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Oturum bilgilerinin saklanacağı önbellek nesnesi
        /// </summary>
        private readonly InMemoryCacheDataProvider _cacheProvider;

        /// <summary>
        /// Kimlik denetimi servisi iletişimcisi
        /// </summary>
        private readonly AuthorizationCommunicator _authorizationCommunicator;

        /// <summary>
        /// Varsayılan kimlik denetimi yapan sınıf
        /// </summary>
        /// <param name="httpContextAccessor">Http üst öğelerine erişim sağlayacak nesne</param>
        /// <param name="cacheProvider">Oturum bilgilerinin saklanacağı önbellek nesnesi</param>
        /// <param name="authorizationCommunicator">Kimlik denetimi servisi iletişimcisi</param>
        public SignalAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor,
            InMemoryCacheDataProvider cacheProvider,
            AuthorizationCommunicator authorizationCommunicator)
        {
            _httpContextAccessor = httpContextAccessor;
            _cacheProvider = cacheProvider;
            _authorizationCommunicator = authorizationCommunicator;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultAuthorizationRequirement requirement)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            AuthenticatedUser user = GetUserFromCache(token);

            if (user != null)
            {
                context.Succeed(requirement);
            }
            else
            {
                if (!string.IsNullOrEmpty(token))
                {
                    ServiceResultModel<global::Communication.Http.Authorization.Models.UserModel> serviceResult = await _authorizationCommunicator.GetUserAsync(token, cancellationTokenSource);

                    if (serviceResult.IsSuccess && serviceResult.Data != null)
                    {
                        AuthenticatedUser userData = new AuthenticatedUser
                        {
                            Email = serviceResult.Data.Email,
                            Id = serviceResult.Data.Id,
                            Password = serviceResult.Data.Password,
                            Region = serviceResult.Data.Region,
                            SessionId = serviceResult.Data.SessionId,
                            Token = serviceResult.Data.Token != null ? new AuthenticationToken()
                            {
                                TokenKey = serviceResult.Data.Token.TokenKey,
                                ValidTo = serviceResult.Data.Token.ValidTo
                            } : null
                        };

                        SetToCache(userData);

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
        /// Token bazlı kullanıcı oturumunu önbellekte saklar
        /// </summary>
        /// <param name="userModel">Kullanıcının model nesnesi</param>
        private void SetToCache(AuthenticatedUser userModel)
        {
            if (_cacheProvider.TryGetValue(CACHEDTOKENBASEDSESSIONS, out List<AuthenticatedUser> cachedUsers) && cachedUsers != default(List<AuthenticatedUser>))
            {
                cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.Now);

                cachedUsers.Add(userModel);

                _cacheProvider.Set(CACHEDTOKENBASEDSESSIONS, cachedUsers);
            }
            else
            {
                List<AuthenticatedUser> userModels = new List<AuthenticatedUser>
                {
                    userModel
                };

                _cacheProvider.Set(CACHEDTOKENBASEDSESSIONS, userModels);
            }
        }

        /// <summary>
        /// Bir kullanıcının geçerli token bazlı oturum bilgisini önbellekten çağırır
        /// </summary>
        /// <param name="token">Kullanıcının oturum anahtarı</param>
        /// <returns></returns>
        private AuthenticatedUser GetUserFromCache(string token)
        {
            if (_cacheProvider.TryGetValue(CACHEDTOKENBASEDSESSIONS, out List<AuthenticatedUser> cachedUsers) && cachedUsers != default(List<AuthenticatedUser>))
            {
                if (cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.Now) > 0)
                {
                    _cacheProvider.Set(CACHEDTOKENBASEDSESSIONS, cachedUsers);
                }

                return cachedUsers.FirstOrDefault(x => x.Token.TokenKey == token);
            }

            return null;
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
                    _authorizationCommunicator.Dispose();
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
