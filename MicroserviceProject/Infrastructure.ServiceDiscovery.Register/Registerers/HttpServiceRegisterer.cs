using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Abstract;
using Infrastructure.ServiceDiscovery.Exceptions;
using Infrastructure.ServiceDiscovery.Models;
using Infrastructure.ServiceDiscovery.Register.Abstract;
using Infrastructure.ServiceDiscovery.Register.Endpoints;
using Infrastructure.ServiceDiscovery.Register.Exceptions;

namespace Infrastructure.ServiceDiscovery.Register.Registerers
{
    public class HttpServiceRegisterer : IServiceRegisterer
    {
        private readonly HttpPostCaller _httpPostCaller;
        private readonly ISolidServiceProvider _solidServiceProvider;

        public HttpServiceRegisterer(
            ISolidServiceProvider solidServiceProvider,
            HttpPostCaller httpPostCaller)
        {
            _solidServiceProvider = solidServiceProvider;
            _httpPostCaller = httpPostCaller;
        }

        public async Task RegisterServiceAsync(ServiceModel service, CancellationTokenSource cancellationTokenSource)
        {
            SolidServiceModel solidService = _solidServiceProvider.GetSolidService();
            if (solidService != null)
            {
                ServiceResultModel serviceResult = await _httpPostCaller.CallAsync<ServiceModel, ServiceResultModel>(new RegisterEndpoint()
                {
                    Url = solidService.RegisterAddress,
                    HttpAction = HttpAction.POST,
                    EndpointAuthentication = new AnonymouseAuthentication()
                }, service, cancellationTokenSource);

                if (serviceResult == null || !serviceResult.IsSuccess)
                    throw new ServiceCouldtNotRegisteredToSolidException(serviceResult?.ErrorModel?.Description ?? "Service couldn't registered to solid");
            }
            else
                throw new SolidServiceNotDefinedException();
        }
    }
}
