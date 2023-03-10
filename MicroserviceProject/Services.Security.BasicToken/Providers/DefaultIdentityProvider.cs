
using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Security.Authentication.BasicToken.Abstracts;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using Services.Communication.Http.Broker.Authorization.Abstract;
using Services.Communication.Http.Broker.Authorization.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Security.BasicToken.Providers
{
    public class DefaultIdentityProvider : IIdentityProvider
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Önbellekte tutulacak token bazlı kullanıcı oturumları için önbellek anahtarı
        /// </summary>
        public const string CACHEDTOKENBASEDSESSIONS = "CACHED_TOKENBASED_SESSIONS";

        /// <summary>
        /// Kimlik denetimi servisi iletişimcisi
        /// </summary>
        private readonly IAuthorizationCommunicator _authorizationCommunicator;

        private readonly InMemoryCacheDataProvider _cacheProvider;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultIdentityProvider(
            IAuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider cacheProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _authorizationCommunicator = authorizationCommunicator;
            _cacheProvider = cacheProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public string AuthenticationFailMessage { get; set; } = "Lütfen tekrar oturum açın";

        /// <summary>
        /// Oturumda bulunan kullanıcı
        /// </summary>
        public async Task<AuthenticatedUser> GetUserAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues headerToken) && headerToken.Any(x => x.Length > 0))
            {
                AuthenticatedUser user = GetUserFromCache(headerToken);

                if (user != null)
                {
                    return user;
                }

                ServiceResultModel<UserModel> serviceResult = await _authorizationCommunicator.GetUserAsync(headerToken, cancellationTokenSource);

                if (serviceResult.Data != null)
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
                        } : null,
                        Claims = serviceResult.Data.Claims.Select(x => new UserClaim()
                        {
                            Name = x.Name,
                            Value = x.Value,
                        }).ToList(),
                        Roles = serviceResult.Data.Roles.Select(x => new UserRole()
                        {
                            Name = x.Name
                        }).ToList()
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
                cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.UtcNow);

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
                if (cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.UtcNow) > 0)
                {
                    _cacheProvider.Set(CACHEDTOKENBASEDSESSIONS, cachedUsers);
                }

                return cachedUsers.FirstOrDefault(x => x.Token.TokenKey == token);
            }

            return null;
        }
    }
}
