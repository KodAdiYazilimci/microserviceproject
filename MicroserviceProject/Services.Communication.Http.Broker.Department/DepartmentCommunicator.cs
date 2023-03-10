using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Security.Authentication.Exceptions;
using Infrastructure.Security.Authentication.Providers;

using Services.Communication.Http.Broker.Abstract;
using Services.Communication.Http.Broker.Authorization.Abstract;
using Services.Communication.Http.Broker.Authorization.Models;
using Services.Communication.Http.Broker.Department.Abstract;

namespace Services.Communication.Http.Broker
{
    public class DepartmentCommunicator : IDepartmentCommunicator
    {
        private const string TAKENTOKENFORTHISSERVICE = "TAKEN_TOKEN_FOR_THIS_SERVICE";

        private readonly ICommunicator _communicator;
        private readonly IAuthorizationCommunicator _authorizationCommunicator;
        private readonly InMemoryCacheDataProvider _cacheProvider;
        private readonly CredentialProvider _credentialProvider;

        public DepartmentCommunicator(
            IAuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider cacheProvider,
            CredentialProvider credentialProvider,
            ICommunicator communicator)
        {
            _authorizationCommunicator = authorizationCommunicator;
            _credentialProvider = credentialProvider;
            _cacheProvider = cacheProvider;
            _communicator = communicator;
        }

        public Task<ServiceResultModel<TResult>> CallAsync<TResult>(IEndpoint endpoint, CancellationTokenSource cancellationTokenSource)
        {
            return _communicator.CallAsync<TResult>(endpoint, cancellationTokenSource);
        }

        public Task<ServiceResultModel<TResult>> CallAsync<TRequest, TResult>(IEndpoint endpoint, TRequest requestObject, CancellationTokenSource cancellationTokenSource)
        {
            return _communicator.CallAsync<TRequest, TResult>(endpoint, requestObject, cancellationTokenSource);
        }

        public async Task<string> GetServiceToken(CancellationTokenSource cancellationTokenSource)
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
