﻿using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Providers.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.IT.Abstract;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.Models;
using Services.Communication.Http.Endpoint.Department.IT;

namespace Services.Communication.Http.Broker.Department.IT
{
    public class ITCommunicator : IITCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly IRouteProvider _routeProvider;
        private readonly IDepartmentCommunicator _departmentCommunicator;

        public ITCommunicator(
            IRouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            _routeProvider = routeProvider;
            _departmentCommunicator = departmentCommunicator;
        }

        public async Task<ServiceResultModel<List<ITInventoryModel>>> GetInventoriesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<ITGetInventoriesEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<List<ITInventoryModel>>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateInventoryAsync(
            ITCreateInventoryCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<ITCreateInventoryEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<ITCreateInventoryCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel<List<ITDefaultInventoryForNewWorkerModel>>> GetInventoriesForNewWorkerAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<ITGetInventoriesForNewWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<List<ITDefaultInventoryForNewWorkerModel>>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> AssignInventoryToWorkerAsync(
           ITAssignInventoryToWorkerCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<ITAssignInventoryToWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<ITAssignInventoryToWorkerCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> CreateDefaultInventoryForNewWorkerAsync(
           ITCreateDefaultInventoryForNewWorkerCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<ITCreateDefaultInventoryForNewWorkerEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<ITCreateDefaultInventoryForNewWorkerCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> InformInventoryRequestAsync(
            ITInformInventoryRequestCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<ITInformInventoryRequestEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<ITInformInventoryRequestCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new GetRouteException();
        }

        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource)
        {
            IEndpoint endpoint = await _routeProvider.GetRoutingEndpointAsync<ITGetInventoriesEndpoint>(cancellationTokenSource);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Queries.Add(new HttpQueryModel() { Name = "tokenKey", Value = tokenKey });

                return await _departmentCommunicator.CallAsync<List<ITInventoryModel>>(authenticatedEndpoint, cancellationTokenSource);
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
