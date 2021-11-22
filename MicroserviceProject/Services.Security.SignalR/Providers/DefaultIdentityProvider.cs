
using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Security.Authentication.SignalR.Abstract;
using Infrastructure.Security.Model;

using Services.Communication.Http.Broker.Authorization;
using Services.Communication.Http.Broker.Authorization.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Security.SignalR.Providers
{
    public class DefaultIdentityProvider : IIdentityProvider
    {
        /// <summary>
        /// Önbellekte tutulacak token bazlı kullanıcı oturumları için önbellek anahtarı
        /// </summary>
        private const string CACHEDTOKENBASEDSESSIONS = "CACHED_TOKENBASED_SESSIONS";

        /// <summary>
        /// Kimlik denetimi servisi iletişimcisi
        /// </summary>
        private readonly AuthorizationCommunicator _authorizationCommunicator;

        /// <summary>
        /// Oturum bilgilerinin saklanacağı önbellek nesnesi
        /// </summary>
        private readonly InMemoryCacheDataProvider _cacheProvider;

        public DefaultIdentityProvider(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider cacheProvider)
        {
            _authorizationCommunicator = authorizationCommunicator;
            _cacheProvider = cacheProvider;
        }

        public async Task<AuthenticatedUser> GetUserAsync(string token, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<UserModel> serviceResult = await _authorizationCommunicator.GetUserAsync(token, cancellationTokenSource);

            if (serviceResult.IsSuccess && serviceResult.Data != null)
            {
                return new AuthenticatedUser()
                {
                    Email = serviceResult.Data.Email,
                    Id = serviceResult.Data.Id,
                    Region = serviceResult.Data.Region,
                    SessionId = serviceResult.Data.SessionId,
                    Roles = serviceResult.Data.Roles.Select(x => new UserRole()
                    {
                        Name = x.Name
                    }).ToList(),
                    Claims = serviceResult.Data.Claims.Select(x => new UserClaim()
                    {
                        Name = x.Name,
                        Value = x.Value
                    }).ToList(),
                    Token = new AuthenticationToken()
                    {
                        RefreshToken = serviceResult.Data.Token.RefreshToken,
                        Scope = serviceResult.Data.Token.Scope,
                        TokenKey = serviceResult.Data.Token.TokenKey,
                        ValidTo = serviceResult.Data.Token.ValidTo
                    }
                };
            }

            return null;
        }

        /// <summary>
        /// Token bazlı kullanıcı oturumunu önbellekte saklar
        /// </summary>
        /// <param name="userModel">Kullanıcının model nesnesi</param>
        public void SetToCache(AuthenticatedUser userModel)
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
        public AuthenticatedUser GetUserFromCache(string token)
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
    }
}
