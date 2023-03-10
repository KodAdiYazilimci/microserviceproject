
using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Security.Authentication.Cookie.Abstract;
using Infrastructure.Security.Authentication.Exceptions;
using Infrastructure.Security.Model;

using Services.Communication.Http.Broker.Authorization.Abstract;
using Services.Communication.Http.Broker.Authorization.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Security.Cookie.Providers
{
    public class DefaultIdentityProvider : IIdentityProvider
    {
        /// <summary>
        /// Önbellekte tutulacak token bazlı kullanıcı oturumları için önbellek anahtarı
        /// </summary>
        private const string CACHEDCOOKIEBASEDSESSIONS = "CACHED_COOKIEBASED_SESSIONS";

        /// <summary>
        /// Kimlik denetimi servisi iletişimcisi
        /// </summary>
        private readonly IAuthorizationCommunicator _authorizationCommunicator;

        private readonly InMemoryCacheDataProvider _cacheProvider;

        public DefaultIdentityProvider(
            IAuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
            _authorizationCommunicator = authorizationCommunicator;
        }

        /// <summary>
        /// Kullanıcı bilgisine göre oturum bilgilerini getirir
        /// </summary>
        /// <param name="credential">Kullanıcı bilgisi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<AuthenticatedUser> GetUserAsync(AuthenticationCredential credential, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<TokenModel> tokenServiceResult =
                await _authorizationCommunicator.GetTokenAsync(new CredentialModel
                {
                    Email = credential.Email,
                    Password = credential.Password,
                    GrantType = credential.GrantType,
                    RefreshToken = credential.RefreshToken,
                    Scope = credential.Scope,
                    IpAddress = credential.IpAddress,
                    Region = credential.Region,
                    UserAgent = credential.UserAgent
                }, cancellationTokenSource);

            if (tokenServiceResult.IsSuccess)
            {
                ServiceResultModel<UserModel> userServiceResult = await _authorizationCommunicator.GetUserAsync(tokenServiceResult.Data.TokenKey, cancellationTokenSource);

                if (userServiceResult.IsSuccess)
                {
                    return new AuthenticatedUser()
                    {
                        Email = userServiceResult.Data.Email,
                        Id = userServiceResult.Data.Id,
                        Region = userServiceResult.Data.Region,
                        SessionId = userServiceResult.Data.SessionId,
                        Token = new AuthenticationToken()
                        {
                            TokenKey = tokenServiceResult.Data.TokenKey,
                            ValidTo = tokenServiceResult.Data.ValidTo,
                            RefreshToken = tokenServiceResult.Data.RefreshToken,
                            Scope = tokenServiceResult.Data.Scope
                        },
                        Claims = userServiceResult.Data.Claims.Select(x => new UserClaim()
                        {
                            Name = x.Name,
                            Value = x.Value,
                        }).ToList(),
                        Roles = userServiceResult.Data.Roles.Select(x => new UserRole()
                        {
                            Name = x.Name
                        }).ToList()
                    };
                }
                else
                    throw new CallException(tokenServiceResult.ErrorModel.Description, tokenServiceResult.SourceApiService);
            }
            else
                throw new CallException(tokenServiceResult.ErrorModel.Description, tokenServiceResult.SourceApiService);
        }

        /// <summary>
        /// Token bilgisine göre oturumda bulunan kullanıcıyı verir
        /// </summary>
        public async Task<AuthenticatedUser> GetUserAsync(string token, CancellationTokenSource cancellationTokenSource)
        {
            AuthenticatedUser authenticatedUser = GetUserFromCache(token);

            if (authenticatedUser != null)
            {
                return authenticatedUser;
            }

            ServiceResultModel<UserModel> userServiceResult = await _authorizationCommunicator.GetUserAsync(token, cancellationTokenSource);

            if (userServiceResult.IsSuccess)
            {
                if (userServiceResult.Data != null)
                {
                    authenticatedUser = new AuthenticatedUser()
                    {
                        Email = userServiceResult.Data.Email,
                        Id = userServiceResult.Data.Id,
                        Region = userServiceResult.Data.Region,
                        SessionId = userServiceResult.Data.SessionId,
                        Token = new AuthenticationToken()
                        {
                            TokenKey = userServiceResult.Data.Token.TokenKey,
                            ValidTo = userServiceResult.Data.Token.ValidTo,
                            RefreshToken = userServiceResult.Data.Token.RefreshToken,
                            Scope = userServiceResult.Data.Token.Scope
                        },
                        Claims = userServiceResult.Data.Claims.Select(x => new UserClaim()
                        {
                            Name = x.Name,
                            Value = x.Value
                        }).ToList(),
                        Roles = userServiceResult.Data.Roles.Select(x => new UserRole()
                        {
                            Name = x.Name
                        }).ToList()
                    };

                    SetToCache(authenticatedUser);

                    return authenticatedUser;
                }
                else
                    return null;
            }
            else
            {
                if (userServiceResult.ErrorModel.Code == ((int)HttpStatusCode.Unauthorized).ToString())
                {
                    throw new SessionNotFoundOrExpiredException();
                }
                else
                    throw new CallException(userServiceResult.ErrorModel.Description, userServiceResult.SourceApiService);
            }
        }

        /// <summary>
        /// Token bazlı kullanıcı oturumunu önbellekte saklar
        /// </summary>
        /// <param name="userModel">Kullanıcının model nesnesi</param>
        public void SetToCache(AuthenticatedUser userModel)
        {
            if (_cacheProvider.TryGetValue(CACHEDCOOKIEBASEDSESSIONS, out List<AuthenticatedUser> cachedUsers) && cachedUsers != default(List<AuthenticatedUser>))
            {
                cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.UtcNow);

                cachedUsers.Add(userModel);

                _cacheProvider.Set(CACHEDCOOKIEBASEDSESSIONS, cachedUsers);
            }
            else
            {
                List<AuthenticatedUser> userModels = new List<AuthenticatedUser>
                {
                    userModel
                };

                _cacheProvider.Set(CACHEDCOOKIEBASEDSESSIONS, userModels);
            }
        }

        /// <summary>
        /// Önbellekten bir kullanıcıyı siler
        /// </summary>
        /// <param name="token">Silinecek kullanıcının tokenı</param>
        public void RemoveUserFromCache(string token)
        {
            if (_cacheProvider.TryGetValue(CACHEDCOOKIEBASEDSESSIONS, out List<AuthenticatedUser> cachedUsers) && cachedUsers != default(List<AuthenticatedUser>))
            {
                if ((cachedUsers.RemoveAll(x => x != null && x.Token != null && x.Token.TokenKey == token) > 0))
                {
                    _cacheProvider.Set(CACHEDCOOKIEBASEDSESSIONS, cachedUsers);
                }
            }
        }

        /// <summary>
        /// Bir kullanıcının geçerli token bazlı oturum bilgisini önbellekten çağırır
        /// </summary>
        /// <param name="token">Kullanıcının oturum anahtarı</param>
        /// <returns></returns>
        public AuthenticatedUser GetUserFromCache(string token)
        {
            if (_cacheProvider.TryGetValue(CACHEDCOOKIEBASEDSESSIONS, out List<AuthenticatedUser> cachedUsers) && cachedUsers != default(List<AuthenticatedUser>))
            {
                if (cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.UtcNow) > 0)
                {
                    _cacheProvider.Set(CACHEDCOOKIEBASEDSESSIONS, cachedUsers);
                }

                return cachedUsers.FirstOrDefault(x => x.Token.TokenKey == token && x.Token.ValidTo > DateTime.UtcNow);
            }

            return null;
        }
    }
}
