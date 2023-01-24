using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.Providers;

using Services.Communication.Http.Broker.Authorization;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.Models;
using Services.Communication.Http.Endpoints.Api.Business.Departments.AA;

namespace Services.Communication.Http.Broker.Department.AA
{
    public class AACommunicator : BaseDepartmentCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly RouteProvider? _routeProvider;

        public AACommunicator(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider cacheProvider,
            CredentialProvider credentialProvider,
            RouteProvider routeProvider,
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller) : base(authorizationCommunicator, cacheProvider, credentialProvider, httpGetCaller, httpPostCaller)
        {
            _routeProvider = routeProvider;
        }

        public async Task<ServiceResultModel<List<InventoryModel>>> GetInventoriesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetInventoriesEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });

                return await CallAsync<List<InventoryModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateInventoryAsync(
            CreateInventoryCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateInventoryEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });
                endpoint.Payload = request;

                return await CallAsync<CreateInventoryCommandRequest, List<InventoryModel>>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel<List<InventoryModel>>> GetInventoriesForNewWorkerAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetInventoriesForNewWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });

                return await CallAsync<List<InventoryModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateDefaultInventoryForNewWorkerAsync(
            CreateDefaultInventoryForNewWorkerCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateDefaultInventoryForNewWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });

                return await CallAsync<CreateDefaultInventoryForNewWorkerCommandRequest, Object>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> AssignInventoryToWorkerAsync(
            AssignInventoryToWorkerCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<AssignInventoryToWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });

                return await CallAsync<AssignInventoryToWorkerCommandRequest, Object>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> InformInventoryRequestAsync(
            InformInventoryRequestCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<InformInventoryRequestEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });

                return await CallAsync<InformInventoryRequestCommandRequest, Object>(endpoint, request, cancellationTokenSource);
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
                endpoint.Queries.Add(new HttpQuery() { Key = "tokenKey", Value = tokenKey });

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
