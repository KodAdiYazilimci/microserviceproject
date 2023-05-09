using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Exceptions;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

using Services.Communication.Http.Broker.Abstract;
using Services.Communication.Http.Broker.ServiceDiscovery.Abstract;
using Services.Communication.Http.Endpoint.ServiceDiscovery;

using System.Threading;

namespace Services.Communication.Http.Broker.ServiceDiscovery
{
    public class ServiceDiscoveryCommunicator : IServiceDiscoveryCommunicator
    {
        private readonly ICommunicator _communicator;
        private readonly IServiceDiscoverer _serviceDiscoverer;

        public ServiceDiscoveryCommunicator(
            ICommunicator communicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            _communicator = communicator;
            _serviceDiscoverer = serviceDiscoverer;
        }

        public async Task<ServiceResultModel> DropServiceAsync(string serviceName, CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.ServiceDiscovery", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(DropServiceEndpoint.Path);

            if (endpoint != null)
            {
                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new AnonymouseAuthentication());

                authenticatedEndpoint.Queries.Add(new HttpQueryModel() { Name = "serviceName", Value = serviceName });

                return await _communicator.CallAsync<DiscoveredServiceModel>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }

        public async Task<ServiceResultModel<List<DiscoveredServiceModel>>> GetDiscoveredServicesAsync(CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.Api.ServiceDiscovery", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(GetDiscoveredServicesEndpoint.Path);

            if (endpoint != null)
            {
                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new AnonymouseAuthentication());

                return await _communicator.CallAsync<List<DiscoveredServiceModel>>(authenticatedEndpoint, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }
    }
}
