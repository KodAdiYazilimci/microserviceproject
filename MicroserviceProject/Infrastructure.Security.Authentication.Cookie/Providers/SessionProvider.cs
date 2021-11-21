using Communication.Http.Authorization;
using Communication.Http.Authorization.Models;

using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker.Exceptions;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Security.Authentication.Exceptions;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Security.Authentication.Cookie.Providers
{
    public class SessionProvider
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Önbellekte tutulacak token bazlı kullanıcı oturumları için önbellek anahtarı
        /// </summary>
        private const string CACHEDCOOKIEBASEDSESSIONS = "CACHED_COOKIEBASED_SESSIONS";

        private readonly InMemoryCacheDataProvider _cacheProvider;

        /// <summary>
        /// Kimlik denetimi servisi iletişimcisi
        /// </summary>
        private readonly AuthorizationCommunicator _authorizationCommunicator;

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        /// <param name="authorizationCommunicator">Kimlik denetimi servisi iletişimcisi</param>
        public SessionProvider(
            AuthorizationCommunicator authorizationCommunicator,
            IHttpContextAccessor httpContextAccessor,
            InMemoryCacheDataProvider cacheProvider)
        {
            _authorizationCommunicator = authorizationCommunicator;
            _cacheProvider = cacheProvider;
            _httpContextAccessor = httpContextAccessor;
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
            AuthenticatedUser authenticatedUser = await GetUserAsync(credential, cancellationTokenSource);

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
                        ExpiresUtc = new DateTimeOffset(authenticatedUser.Token.ValidTo.ToUniversalTime())
                    });

                SetToCache(authenticatedUser);

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
            AuthenticatedUser authenticatedUser = await GetUserAsync(token, cancellationTokenSource);

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
                        ExpiresUtc = authenticatedUser.Token.ValidTo.ToUniversalTime()
                    });

                SetToCache(authenticatedUser);

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
                AuthenticatedUser authenticatedUser = GetUserFromCache(claim.Value);

                if (authenticatedUser != null)
                {
                    RemoveUserFromCache(authenticatedUser.Token.TokenKey);
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
                return await GetUserAsync(claim.Value, cancellationTokenSource);
            }

            return null;
        }

        /// <summary>
        /// Kullanıcı bilgisine göre oturum bilgilerini getirir
        /// </summary>
        /// <param name="credential">Kullanıcı bilgisi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        private async Task<AuthenticatedUser> GetUserAsync(AuthenticationCredential credential, CancellationTokenSource cancellationTokenSource)
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
                        Token = new Model.AuthenticationToken()
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
                        Token = new Model.AuthenticationToken()
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
        private void SetToCache(AuthenticatedUser userModel)
        {
            if (_cacheProvider.TryGetValue(CACHEDCOOKIEBASEDSESSIONS, out List<AuthenticatedUser> cachedUsers) && cachedUsers != default(List<AuthenticatedUser>))
            {
                cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.Now);

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
        private void RemoveUserFromCache(string token)
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
        private AuthenticatedUser GetUserFromCache(string token)
        {
            if (_cacheProvider.TryGetValue(CACHEDCOOKIEBASEDSESSIONS, out List<AuthenticatedUser> cachedUsers) && cachedUsers != default(List<AuthenticatedUser>))
            {
                if (cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.Now) > 0)
                {
                    _cacheProvider.Set(CACHEDCOOKIEBASEDSESSIONS, cachedUsers);
                }

                return cachedUsers.FirstOrDefault(x => x.Token.TokenKey == token && x.Token.ValidTo > DateTime.Now);
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
