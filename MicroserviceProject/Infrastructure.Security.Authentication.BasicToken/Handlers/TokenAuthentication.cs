using Communication.Http.Authorization;

using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Security.Authentication.BasicToken.Schemes;
using Infrastructure.Security.Authentication.Persistence;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
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
    public class TokenAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Önbellekte tutulacak token bazlı kullanıcı oturumları için önbellek anahtarı
        /// </summary>
        private const string CACHEDTOKENBASEDSESSIONS = "CACHED_TOKENBASED_SESSIONS";

        private readonly InMemoryCacheDataProvider _cacheProvider;

        /// <summary>
        /// Kimlik denetimi servisi iletişimcisi
        /// </summary>
        private readonly AuthorizationCommunicator _authorizationCommunicator;

        /// <summary>
        /// Kimlik doğrulama denetimi yapacak sınıf
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="urlEncoder"></param>
        /// <param name="systemClock"></param>
        /// <param name="authorizationCommunicator">Kimlik denetimi servisi iletişimcisi</param>
        /// <param name="cacheProvider">Önbellek sağlayıcısı sınıf</param>
        public TokenAuthentication(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            ISystemClock systemClock,
            InMemoryCacheDataProvider cacheProvider,
            AuthorizationCommunicator authorizationCommunicator) : base(options, loggerFactory, urlEncoder, systemClock)
        {
            _cacheProvider = cacheProvider;
            _authorizationCommunicator = authorizationCommunicator;
        }

        /// <summary>
        /// Kimlik denetimini sağlar
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            if (await GetUserAsync(cancellationTokenSource) != null)
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(ClaimProvider.GetClaims(await GetUserAsync(cancellationTokenSource)), Default.DefaultScheme);

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Default.DefaultScheme));
            }
            else
            {
                return AuthenticateResult.Fail("Lütfen tekrar oturum açın!");
            }
        }

        /// <summary>
        /// Oturumda bulunan kullanıcı
        /// </summary>
        protected async Task<AuthenticatedUser> GetUserAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (Request.Headers.TryGetValue("Authorization", out StringValues headerToken) && headerToken.Any(x => x.Length > 0))
            {
                AuthenticatedUser user = GetUserFromCache(headerToken);

                if (user != null)
                {
                    return user;
                }

                ServiceResultModel<global::Communication.Http.Authorization.Models.User> serviceResult = await _authorizationCommunicator.GetUserAsync(headerToken, cancellationTokenSource);

                if (serviceResult.Data != null)
                {
                    AuthenticatedUser userData = new AuthenticatedUser
                    {
                        Email = serviceResult.Data.Email,
                        Id = serviceResult.Data.Id,
                        IsAdmin = serviceResult.Data.IsAdmin,
                        Name = serviceResult.Data.Name,
                        Password = serviceResult.Data.Password,
                        Region = serviceResult.Data.Region,
                        SessionId = serviceResult.Data.SessionId,
                        Token = serviceResult.Data.Token != null ? new Model.AuthenticationToken()
                        {
                            TokenKey = serviceResult.Data.Token.TokenKey,
                            ValidTo = serviceResult.Data.Token.ValidTo
                        } : null
                    };

                    if (serviceResult.IsSuccess)
                    {
                        SetToCache(userData);
                    }

                    return userData;
                }
            }

            //throw new SessionExpiredException("Lütfen tekrar oturum açın!");
            return null;
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
                    _authorizationCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
