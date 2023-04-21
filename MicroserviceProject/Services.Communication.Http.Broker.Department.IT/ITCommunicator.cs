using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Exceptions;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

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

        private readonly IDepartmentCommunicator _departmentCommunicator;
        private readonly IServiceDiscoverer _serviceDiscoverer;

        public ITCommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            _departmentCommunicator = departmentCommunicator;
            _serviceDiscoverer = serviceDiscoverer;
        }

        public async Task<ServiceResultModel<List<ITInventoryModel>>> GetInventoriesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.IT", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(ITGetInventoriesEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<List<ITInventoryModel>>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> CreateInventoryAsync(
            ITCreateInventoryCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.IT", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(ITCreateInventoryEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<ITCreateInventoryCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel<List<ITDefaultInventoryForNewWorkerModel>>> GetInventoriesForNewWorkerAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.IT", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(ITGetInventoriesForNewWorkerEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<List<ITDefaultInventoryForNewWorkerModel>>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> AssignInventoryToWorkerAsync(
           ITAssignInventoryToWorkerCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.IT", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(ITAssignInventoryToWorkerEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<ITAssignInventoryToWorkerCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> CreateDefaultInventoryForNewWorkerAsync(
           ITCreateDefaultInventoryForNewWorkerCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.IT", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(ITCreateDefaultInventoryForNewWorkerEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<ITCreateDefaultInventoryForNewWorkerCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> InformInventoryRequestAsync(
            ITInformInventoryRequestCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.IT", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(ITInformInventoryRequestEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<ITInformInventoryRequestCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.IT", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(ITRemoveSessionIfExistsInCacheEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Queries.Add(new HttpQueryModel() { Name = "tokenKey", Value = tokenKey });

                return await _departmentCommunicator.CallAsync<List<ITInventoryModel>>(authenticatedEndpoint, cancellationTokenSource);
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
