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
using Services.Communication.Http.Broker.Department.Accounting.Endpoints;
using Services.Communication.Http.Broker.Department.Accounting.Models;

namespace Services.Communication.Http.Broker.Department.Accounting
{
    public class AccountingCommunicator : BaseDepartmentCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly RouteProvider? _routeProvider;

        public AccountingCommunicator(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider cacheProvider,
            CredentialProvider credentialProvider,
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller,
            RouteProvider? routeProvider) : base(authorizationCommunicator, cacheProvider, credentialProvider, httpGetCaller, httpPostCaller)
        {
            _routeProvider = routeProvider;
        }

        public async Task<ServiceResultModel<List<AccountingBankAccountModel>>> GetBankAccountsOfWorkerAsync(
            int workerId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetBankAccountsOfWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;
                endpoint.Queries["workerId"] = workerId.ToString();

                return await CallAsync<List<AccountingBankAccountModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateBankAccountAsync(
            AccountingCreateBankAccountCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateBankAccountEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await CallAsync<AccountingCreateBankAccountCommandRequest, Object>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel<List<AccountingCurrencyModel>>> GetCurrenciesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetCurrenciesEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await CallAsync<List<AccountingCurrencyModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateCurrencyAsync(
            AccountingCreateCurrencyCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateCurrencyEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await CallAsync<AccountingCreateCurrencyCommandRequest, Object>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel<List<AccountingSalaryPaymentModel>>> GetSalaryPaymentsOfWorkerAsync(
            int workerId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<GetSalaryPaymentsOfWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;
                endpoint.Queries["workerId"] = workerId.ToString();

                return await CallAsync<List<AccountingSalaryPaymentModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateSalaryPaymentAsync(
           AccountingCreateSalaryPaymentCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint? endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateSalaryPaymentEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await CallAsync<AccountingCreateSalaryPaymentCommandRequest, Object>(endpoint, request, cancellationTokenSource);
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
