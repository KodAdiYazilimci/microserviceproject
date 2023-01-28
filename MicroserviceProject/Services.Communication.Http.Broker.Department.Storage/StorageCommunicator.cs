using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.Providers;

using Services.Communication.Http.Broker.Authorization;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Storage.Models;
using Services.Communication.Http.Endpoints.Api.Business.Departments.Stock;

namespace Services.Communication.Http.Broker.Department.Storage
{
    public class StorageCommunicator : BaseDepartmentCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly RouteProvider? _routeProvider;

        public StorageCommunicator(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider cacheProvider,
            CredentialProvider credentialProvider,
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller,
            RouteProvider? routeProvider) : base(authorizationCommunicator, cacheProvider, credentialProvider, httpGetCaller, httpPostCaller)
        {
            _routeProvider = routeProvider;
        }

        public async Task<ServiceResultModel<StockModel>> GetStockAsync(
            int productId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetStockEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;
                endpoint.Queries["productId"] = productId.ToString();

                return await CallAsync<StockModel>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> DescendStockAsync(
           DescendProductStockCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<DescendStockEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await CallAsync<DescendProductStockCommandRequest, Object>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateStockAsync(
           CreateStockCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateStockEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await CallAsync<CreateStockCommandRequest, Object>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<RemoveSessionIfExistsInCacheEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Queries["tokenKey"] = tokenKey;

                return await CallAsync<Object>(endpoint, cancellationTokenSource);
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
