using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.Providers;

using Services.Communication.Http.Broker.Authorization;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.Models;
using Services.Communication.Http.Endpoints.Api.Business.Departments.Accounting;

namespace Services.Communication.Http.Broker.Department.Accounting
{
    public class NewAccountingCommunicator : BaseDepartmentCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly RouteProvider? _routeProvider;

        public NewAccountingCommunicator(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider cacheProvider,
            CredentialProvider credentialProvider,
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller,
            RouteProvider? routeProvider) : base(authorizationCommunicator, cacheProvider, credentialProvider, httpGetCaller, httpPostCaller)
        {
            _routeProvider = routeProvider;
        }

        public async Task<ServiceResultModel<List<BankAccountModel>>> GetBankAccountsOfWorkerAsync(
            int workerId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetBankAccountsOfWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });
                endpoint.Queries.Add(new HttpQuery() { Key = "workerId", Value = workerId.ToString() });

                return await CallAsync<List<BankAccountModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateBankAccountAsync(
            CreateBankAccountCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateBankAccountEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });

                return await CallAsync<CreateBankAccountCommandRequest, Object>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel<List<CurrencyModel>>> GetCurrenciesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetCurrenciesEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });

                return await CallAsync<List<CurrencyModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateCurrencyAsync(
            CreateCurrencyCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateCurrencyEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });

                return await CallAsync<CreateCurrencyCommandRequest, Object>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel<List<SalaryPaymentModel>>> GetSalaryPaymentsOfWorkerAsync(
            int workerId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetSalaryPaymentsOfWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });
                endpoint.Queries.Add(new HttpQuery() { Key = "workerId", Value = workerId.ToString() });

                return await CallAsync<List<SalaryPaymentModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateSalaryPaymentAsync(
           CreateSalaryPaymentCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateSalaryPaymentEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers.Add(new HttpHeader() { Key = "TransactionIdentity", Value = transactionIdentity });

                return await CallAsync<CreateSalaryPaymentCommandRequest, Object>(endpoint, request, cancellationTokenSource);
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
