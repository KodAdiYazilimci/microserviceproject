using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Abstract;
using Infrastructure.ServiceDiscovery.Register.Abstract;
using Infrastructure.ServiceDiscovery.Register.Endpoints;
using Infrastructure.ServiceDiscovery.Register.Exceptions;
using Infrastructure.ServiceDiscovery.Register.Models;

namespace Infrastructure.ServiceDiscovery.Register.Registerers
{
    public class HttpServiceRegisterer : IServiceRegisterer
    {
        private readonly HttpPostCaller _httpPostCaller;
        private readonly ISolidServiceConfiguration _solidServiceConfiguration;

        public HttpServiceRegisterer(
            ISolidServiceConfiguration solidServiceConfiguration,
            HttpPostCaller httpPostCaller)
        {
            _solidServiceConfiguration = solidServiceConfiguration;
            _httpPostCaller = httpPostCaller;
        }

        public async Task RegisterServiceAsync(RegisteredServiceModel service, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel serviceResult = await _httpPostCaller.CallAsync<RegisteredServiceModel, ServiceResultModel>(new RegisterEndpoint()
            {
                Url = _solidServiceConfiguration.RegisterAddress,
                HttpAction = HttpAction.POST,
                EndpointAuthentication = new AnonymouseAuthentication()
            }, service, cancellationTokenSource);

            if (serviceResult == null || !serviceResult.IsSuccess)
                throw new ServiceCouldtNotRegisteredToSolidException(serviceResult?.ErrorModel?.Description ?? "Service couldn't registered to solid");
        }
    }
}
