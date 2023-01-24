using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Security.Authentication.Exceptions;
using Infrastructure.Security.Authentication.Providers;

using Services.Communication.Http.Broker.Authorization;
using Services.Communication.Http.Broker.Authorization.Models;

namespace Services.Communication.Http.Broker
{
    public class BaseDepartmentCommunicator : BaseCommunicator
    {
        private const string TAKENTOKENFORTHISSERVICE = "TAKEN_TOKEN_FOR_THIS_SERVICE";

        private readonly AuthorizationCommunicator _authorizationCommunicator;
        private readonly InMemoryCacheDataProvider _cacheProvider;
        private readonly CredentialProvider _credentialProvider;

        public BaseDepartmentCommunicator(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider cacheProvider,
            CredentialProvider credentialProvider,
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller) : base(httpGetCaller, httpPostCaller)
        {
            _authorizationCommunicator = authorizationCommunicator;
            _credentialProvider = credentialProvider;
            _cacheProvider = cacheProvider;
        }

        protected async Task<string> GetServiceToken(CancellationTokenSource cancellationTokenSource)
        {
            if (_cacheProvider.TryGetValue<TokenModel>(TAKENTOKENFORTHISSERVICE, out TokenModel takenTokenForThisService)
                &&
                takenTokenForThisService.ValidTo >= DateTime.UtcNow)
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
