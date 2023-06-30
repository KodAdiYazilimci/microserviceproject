using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Exceptions;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

using Services.Communication.WebSockets.Broker.Abstract;
using Services.Communication.WebSockets.Broker.Security.Abstract;
using Services.Communication.WebSockets.Broker.Security.CQRS.Requests;
using Services.Communication.WebSockets.Broker.Security.CQRS.Responses;
using Services.Communication.WebSockets.Endpoint.Security;

namespace Services.Communication.WebSockets.Broker.Security
{
    public class SecurityCommunicator : ISecurityCommunicator
    {
        private readonly ISocketCommunicator _socketCommunicator;
        private readonly IServiceDiscoverer _serviceDiscoverer;

        public SecurityCommunicator(
            ISocketCommunicator socketCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            _socketCommunicator = socketCommunicator;
            _serviceDiscoverer = serviceDiscoverer;
        }

        public async Task<ServiceResultModel<SendTokenNotificationResponse>> SendTokenNotificationAsync(
            SendTokenNotificationRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Services.WebSockets.Security", cancellationTokenSource);

            IEndpoint endpoint = service.GetEndpoint(SendTokenNotificationEndpoint.Path);

            if (endpoint != null)
            {
                string token = await _socketCommunicator.GetServiceToken(cancellationTokenSource);

                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint(new TokenAuthentication(token));
                authenticatedEndpoint.Headers.Add(new HttpHeaderModel() { Name = "TransactionIdentity", Value = transactionIdentity });

                return await _socketCommunicator.CallAsync<SendTokenNotificationRequest, SendTokenNotificationResponse>(authenticatedEndpoint, request, cancellationTokenSource);
            }
            else
                throw new EndpointNotFoundException();
        }
    }
}
