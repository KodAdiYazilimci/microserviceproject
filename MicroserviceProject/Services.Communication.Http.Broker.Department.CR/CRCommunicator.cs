using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Providers.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.CR.Abstract;
using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.CR.Models;
using Services.Communication.Http.Endpoint.Department.CR;

namespace Services.Communication.Http.Broker.Department.CR
{
    public class CRCommunicator : ICRCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly IRouteProvider _routeProvider;
        private readonly IDepartmentCommunicator _departmentCommunicator;

        public CRCommunicator(
            IRouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            _routeProvider = routeProvider;
            _departmentCommunicator = departmentCommunicator;
        }

        public async Task<ServiceResultModel<List<CustomerModel>>> GetCustomersAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<GetCustomersEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<List<CustomerModel>>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateCustomerAsync(
            CreateCustomerCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateCustomerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));

                authenticatedEndpoint.EndpointAuthentication = new TokenAuthentication(token);
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<CreateCustomerCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<RemoveSessionIfExistsInCacheEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));

                authenticatedEndpoint.EndpointAuthentication = new TokenAuthentication(token);
                authenticatedEndpoint.Queries.Add(new HttpQueryModel() { Name = "tokenKey", Value = tokenKey });

                return await _departmentCommunicator.CallAsync<Object>(authenticatedEndpoint, cancellationTokenSource);
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
