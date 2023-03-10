﻿using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Accounting.Abstract;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.Endpoints;
using Services.Communication.Http.Broker.Department.Accounting.Models;

namespace Services.Communication.Http.Broker.Department.Accounting
{
    public class AccountingCommunicator : IAccountingCommunicator
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly RouteProvider _routeProvider;
        private readonly IDepartmentCommunicator _departmentCommunicator;

        public AccountingCommunicator(
            RouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            _routeProvider = routeProvider;
            _departmentCommunicator = departmentCommunicator;
        }

        public async Task<ServiceResultModel<List<AccountingBankAccountModel>>> GetBankAccountsOfWorkerAsync(
            int workerId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<GetBankAccountsOfWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;
                endpoint.Queries["workerId"] = workerId.ToString();

                return await _departmentCommunicator.CallAsync<List<AccountingBankAccountModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateBankAccountAsync(
            AccountingCreateBankAccountCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateBankAccountEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await _departmentCommunicator.CallAsync<AccountingCreateBankAccountCommandRequest, Object>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel<List<AccountingCurrencyModel>>> GetCurrenciesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<GetCurrenciesEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await _departmentCommunicator.CallAsync<List<AccountingCurrencyModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateCurrencyAsync(
            AccountingCreateCurrencyCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateCurrencyEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await _departmentCommunicator.CallAsync<AccountingCreateCurrencyCommandRequest, Object>(endpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel<List<AccountingSalaryPaymentModel>>> GetSalaryPaymentsOfWorkerAsync(
            int workerId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<GetSalaryPaymentsOfWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;
                endpoint.Queries["workerId"] = workerId.ToString();

                return await _departmentCommunicator.CallAsync<List<AccountingSalaryPaymentModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateSalaryPaymentAsync(
           AccountingCreateSalaryPaymentCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<CreateSalaryPaymentEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Headers["TransactionIdentity"] = transactionIdentity;

                return await _departmentCommunicator.CallAsync<AccountingCreateSalaryPaymentCommandRequest, Object>(endpoint, request, cancellationTokenSource);
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

                endpoint.EndpointAuthentication = new TokenAuthentication(token);
                endpoint.Queries["tokenKey"] = tokenKey;

                return await _departmentCommunicator.CallAsync<Object>(endpoint, cancellationTokenSource);
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
