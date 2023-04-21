using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Exceptions;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Selling.Abstract;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Selling.Models;
using Services.Communication.Http.Endpoint.Department.Selling;

namespace Services.Communication.Http.Broker.Department.Selling
{
    public class SellingCommunicator : ISellingCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly IDepartmentCommunicator _departmentCommunicator;
        private readonly IServiceDiscoverer _serviceDiscoverer;

        public SellingCommunicator(
            IDepartmentCommunicator departmentCommunicator, 
            IServiceDiscoverer serviceDiscoverer)
        {
            _departmentCommunicator = departmentCommunicator;
            _serviceDiscoverer = serviceDiscoverer;
        }

        public async Task<ServiceResultModel<List<SellModel>>> GetSoldsAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.Selling", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(GetSoldsEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<List<SellModel>>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> CreateSellingAsync(
            CreateSellingCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.Selling", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(CreateSellingEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<CreateSellingCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> NotifyProductionRequestAsync(
            NotifyProductionRequestCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.Selling", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(NotifyProductionRequestEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<NotifyProductionRequestCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.Selling", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(RemoveSessionIfExistsInCacheEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Queries.Add(new HttpQueryModel() { Name = "tokenKey", Value = tokenKey });

                return await _departmentCommunicator.CallAsync<Object>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
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