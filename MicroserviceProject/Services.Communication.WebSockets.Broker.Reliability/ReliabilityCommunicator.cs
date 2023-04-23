using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Exceptions;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

using Services.Communication.WebSockets.Broker.Abstract;
using Services.Communication.WebSockets.Broker.Reliability.Abstract;
using Services.Communication.WebSockets.Broker.Reliability.CQRS.Commands.Requests;
using Services.Communication.WebSockets.Broker.Reliability.CQRS.Commands.Responses;
using Services.Communication.WebSockets.Endpoint.Reliability;

namespace Services.Communication.WebSockets.Broker.Reliability
{
    public class ReliabilityCommunicator : IReliabilityCommunicator
    {
        private readonly ISocketCommunicator _socketCommunicator;
        private readonly IServiceDiscoverer _serviceDiscoverer;

        public ReliabilityCommunicator(
            ISocketCommunicator socketCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            _socketCommunicator = socketCommunicator;
            _serviceDiscoverer = serviceDiscoverer;
        }

        public async Task<ServiceResultModel<SendErrorNotificationResponse>> SendErrorNotificationAsync(
            SendErrorNotificationRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.WebSockets.Reliability", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(SendErrorNotificationEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _socketCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _socketCommunicator.CallAsync<SendErrorNotificationRequest, SendErrorNotificationResponse>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }
    }
}
