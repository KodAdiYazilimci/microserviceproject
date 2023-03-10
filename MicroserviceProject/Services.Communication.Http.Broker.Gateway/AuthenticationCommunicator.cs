using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Security.Authentication.Abstract;
using Infrastructure.Security.Authentication.Exceptions;

using Services.Communication.Http.Broker.Authorization.Abstract;
using Services.Communication.Http.Broker.Authorization.Models;
using Services.Communication.Http.Broker.Gateway.Abstract;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Gateway
{
    public class AuthenticationCommunicator : IAuthenticationCommunicator
    {
        private const string TAKENTOKENFORTHISSERVICE = "TAKEN_TOKEN_FOR_THIS_SERVICE";

        private readonly IAuthorizationCommunicator _authorizationCommunicator;
        private readonly InMemoryCacheDataProvider _cacheProvider;
        private readonly ICredentialProvider _credentialProvider;

        public AuthenticationCommunicator(
            IAuthorizationCommunicator authorizationCommunicator, 
            InMemoryCacheDataProvider cacheProvider, 
            ICredentialProvider credentialProvider)
        {
            _authorizationCommunicator = authorizationCommunicator;
            _cacheProvider = cacheProvider;
            _credentialProvider = credentialProvider;
        }

        public async Task<string> GetServiceToken(CancellationTokenSource cancellationTokenSource)
        {
            if (_cacheProvider.TryGetValue<TokenModel>(TAKENTOKENFORTHISSERVICE, out TokenModel takenTokenForThisService)
                &&
                takenTokenForThisService != null && takenTokenForThisService.ValidTo >= DateTime.UtcNow)
            {
                return takenTokenForThisService.TokenKey;
            }
            else
            {
                ServiceResultModel<TokenModel> tokenResult = await _authorizationCommunicator.GetTokenAsync(new CredentialModel()
                {
                    Email = _credentialProvider.GetEmail,
                    Password = _credentialProvider.GetPassword
                }, cancellationTokenSource);

                if (tokenResult != null && tokenResult.IsSuccess && tokenResult.Data != null)
                {
                    _cacheProvider.Set<TokenModel>(TAKENTOKENFORTHISSERVICE, tokenResult.Data, tokenResult.Data.ValidTo.AddMinutes(-1));

                    return tokenResult.Data.TokenKey;
                }
                else
                {
                    throw new GetTokenException("Kaynak servis yetki tokenı elde edilemedi");
                }
            }
        }
    }
}
