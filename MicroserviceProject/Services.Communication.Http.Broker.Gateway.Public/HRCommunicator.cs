using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Providers.Abstract;

using Services.Communication.Http.Broker.Abstract;
using Services.Communication.Http.Broker.Gateway.Abstract;
using Services.Communication.Http.Broker.Gateway.Endpoints;
using Services.Communication.Http.Broker.Gateway.Gateway.Public.Abstract;
using Services.Communication.Http.Broker.Gateway.Public.Models;

namespace Services.Communication.Http.Broker.Gateway.Public
{
    public class HRCommunicator : IHRCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly ICommunicator _communicator;
        private readonly IRouteProvider _routeProvider;
        private readonly IAuthenticationCommunicator _authenticationCommunicator;

        public HRCommunicator(
            ICommunicator communicator,
            IRouteProvider routeProvider,
            IAuthenticationCommunicator authenticationCommunicator)
        {
            _routeProvider = routeProvider;
            _authenticationCommunicator = authenticationCommunicator;
            _communicator = communicator;
        }

        public async Task<ServiceResultModel<List<DepartmentModel>>> GetDepartmentsAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<GetDepartmentsEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _authenticationCommunicator.GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await _communicator.CallAsync<List<DepartmentModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(string tokenKey, CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<RemoveSessionIfExistsInCacheEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _authenticationCommunicator.GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Queries["tokenKey"] = tokenKey;

                return await _communicator.CallAsync<Object>(endpoint, cancellationTokenSource);
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
