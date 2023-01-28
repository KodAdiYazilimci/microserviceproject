using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Authorization.Models;
using Services.Communication.Http.Endpoints.Api.Authorization;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Authorization
{
    public class AuthorizationCommunicator : BaseCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly RouteProvider? _routeProvider;

        public AuthorizationCommunicator(
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller,
            RouteProvider routeProvider) : base(httpGetCaller, httpPostCaller)
        {
            _routeProvider = routeProvider;
        }

        public async Task<ServiceResultModel<TokenModel>> GetTokenAsync(
            CredentialModel credential,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetTokenEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                endpoint.EndpointAuthentication = new AnonymouseAuthentication();

                return await CallAsync<CredentialModel, TokenModel>(endpoint, credential, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel<UserModel>> GetUserAsync(
            string headerToken, 
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetUserEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                endpoint.EndpointAuthentication = new AnonymouseAuthentication();
                endpoint.Queries["token"] = headerToken;   

                return await CallAsync<UserModel>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
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

                }

                disposed = true;
            }
        }
    }
}
