using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Security.BasicToken.Providers;

using System;
using System.Collections.Generic;

namespace Services.Api.Business.Departments.HR.Controllers
{
    [Authorize]
    [Route("Identity")]
    public class IdentityController : BaseController
    {
        private readonly InMemoryCacheDataProvider _cacheProvider;

        public IdentityController(InMemoryCacheDataProvider cacheDataProvider)
        {
            _cacheProvider = cacheDataProvider;
        }

        [Route(nameof(RemoveSessionIfExistsInCache))]
        public IActionResult RemoveSessionIfExistsInCache(string tokenKey)
        {
            return HttpResponseWrapper.Wrap(() =>
            {
                if (_cacheProvider.TryGetValue(DefaultIdentityProvider.CACHEDTOKENBASEDSESSIONS, out List<AuthenticatedUser> cachedUsers)
                    &&
                    cachedUsers != default(List<AuthenticatedUser>))
                {
                    if (cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.Now || x.Token.TokenKey == tokenKey) > 0)
                    {
                        _cacheProvider.Set(DefaultIdentityProvider.CACHEDTOKENBASEDSESSIONS, cachedUsers);
                    }
                }
            });
        }
    }
}
