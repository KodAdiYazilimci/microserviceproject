using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Exceptions;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

using Services.Communication.Http.Broker.Department.AA.Abstract;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.Models;
using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Endpoint.Department.AA;

namespace Services.Communication.Http.Broker.Department.AA
{
    public class AACommunicator : IAACommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly IDepartmentCommunicator _departmentCommunicator;
        private readonly IServiceDiscoverer _serviceDiscoverer;

        public AACommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            _departmentCommunicator = departmentCommunicator;
            _serviceDiscoverer = serviceDiscoverer;
        }

        public async Task<ServiceResultModel<List<AAInventoryModel>>> GetInventoriesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.AA", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(AAGetInventoriesEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));

                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<List<AAInventoryModel>>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> CreateInventoryAsync(
            AACreateInventoryCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.AA", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(AACreateInventoryEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });
                authenticatedEndpoint.Payload = request;

                return await _departmentCommunicator.CallAsync<AACreateInventoryCommandRequest, List<AAInventoryModel>>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel<List<AADefaultInventoryForNewWorkerModel>>> GetInventoriesForNewWorkerAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.AA", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(AAGetInventoriesEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<List<AADefaultInventoryForNewWorkerModel>>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> CreateDefaultInventoryForNewWorkerAsync(
            AACreateDefaultInventoryForNewWorkerCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.AA", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(AACreateDefaultInventoryForNewWorkerEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<AACreateDefaultInventoryForNewWorkerCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> AssignInventoryToWorkerAsync(
            AAAssignInventoryToWorkerCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.AA", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(AAAssignInventoryToWorkerEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<AAAssignInventoryToWorkerCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> InformInventoryRequestAsync(
            AAInformInventoryRequestCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.AA", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(AAInformInventoryRequestEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<AAInformInventoryRequestCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.AA", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(AARemoveSessionIfExistsInCacheEndpoint.Path);

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
