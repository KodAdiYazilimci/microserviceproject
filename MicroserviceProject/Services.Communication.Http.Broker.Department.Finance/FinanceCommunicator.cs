using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Exceptions;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Finance.Abstract;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Finance.Models;
using Services.Communication.Http.Endpoint.Department.Finance;

namespace Services.Communication.Http.Broker.Department.Finance
{
    public class FinanceCommunicator : IFinanceCommunicator, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly IDepartmentCommunicator _departmentCommunicator;
        private readonly IServiceDiscoverer _serviceDiscoverer;

        public FinanceCommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceProvider)
        {
            _departmentCommunicator = departmentCommunicator;
            _serviceDiscoverer = serviceProvider;
        }

        public async Task<ServiceResultModel> CreateCostAsync(
            CreateCostCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.Finance", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(CreateCostEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));

                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<CreateCostCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> CreateProductionRequestAsync(
           CreateProductionRequestCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.Finance", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(CreateProductionRequestEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<CreateProductionRequestCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel<List<DecidedCostModel>>> GetDecidedCostsAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.Finance", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(GetDecidedCostsEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));

                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<List<DecidedCostModel>>(endpoint, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> DecideCostAsync(
            DecideCostCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.Finance", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(DecideCostEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _departmentCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _departmentCommunicator.CallAsync<DecideCostCommandRequest, Object>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.Business.Departments.Finance", cancellationTokenSource);

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
