using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Providers.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

using Services.Communication.Http.Broker.Abstract;
using Services.Communication.Http.Broker.Authorization.Abstract;
using Services.Communication.Http.Broker.Authorization.Endpoints;
using Services.Communication.Http.Broker.Authorization.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Authorization
{
    public class AuthorizationCommunicator : IAuthorizationCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly ICommunicator _communicator;
        //private readonly IRouteProvider _routeProvider;
        private readonly IServiceDiscoverer _serviceDiscoverer;

        public AuthorizationCommunicator(
            //IRouteProvider routeProvider,
            ICommunicator communicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            //_routeProvider = routeProvider;
            _communicator = communicator;
            _serviceDiscoverer = serviceDiscoverer;
        }

        public async Task<ServiceResultModel<TokenModel>> GetTokenAsync(
            CredentialModel credential,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Authorization", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint("GetToken");

            if (endpoint != null)
            {
                endpoint.EndpointAuthentication = new AnonymouseAuthentication();

                return await _communicator.CallAsync<CredentialModel, TokenModel>(endpoint, credential, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel<UserModel>> GetUserAsync(
            string headerToken,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Authorization", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint("authorization.auth.gettoken");

            if (endpoint != null)
            {
                endpoint.EndpointAuthentication = new AnonymouseAuthentication();
                endpoint.Queries.Add(new HttpQueryModel() { Name = "token", Value = headerToken });

                return await _communicator.CallAsync<object, UserModel>(endpoint, null, cancellationTokenSource);
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
