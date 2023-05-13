using Hangfire.Common;

using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

using Services.Communication.Http.Broker.Abstract;
using Services.Communication.Http.Broker.ServiceDiscovery.Abstract;
using Services.Scheduling.Diagnostics.HealthCheck.Persistence;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Scheduling.Diagnostics.HealthCheck.Jobs
{
    public class CheckApiServicesJob
    {
        private readonly ICommunicator _communicator;
        private readonly IServiceDiscoverer _serviceDiscoverer;
        private readonly IServiceDiscoveryCommunicator _serviceDiscoveryCommunicator;
        private readonly TempData _tempData;


        public CheckApiServicesJob(
            ICommunicator communicator,
            IServiceDiscoveryCommunicator serviceDiscoveryCommunicator,
            IServiceDiscoverer serviceDiscoverer,
            TempData tempData)
        {
            _communicator = communicator;
            _serviceDiscoverer = serviceDiscoverer;
            _serviceDiscoveryCommunicator = serviceDiscoveryCommunicator;
            _tempData = tempData;
        }

        public async Task CheckServicesAsync()
        {
            try
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                var discoveredServices = await _serviceDiscoveryCommunicator.GetDiscoveredServicesAsync(cancellationTokenSource);

                if (discoveredServices != null && discoveredServices.IsSuccess)
                {
                    foreach (DiscoveredServiceModel discoveredService in discoveredServices.Data)
                    {
                        CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync(discoveredService.ServiceName, cancellationTokenSource);

                        if (service != null)
                        {
                            IEndpoint endpoint = service.GetEndpoint(x => x.Name.Contains("health"));

                            if (endpoint != null)
                            {
                                if (endpoint.HttpAction == HttpAction.GET)
                                {
                                    ServiceResultModel<object> healthCheckResult = await _communicator.CallAsync<object>(endpoint.ConvertToAuthenticatedEndpoint(new AnonymouseAuthentication()), cancellationTokenSource);

                                    if (!healthCheckResult.IsSuccess)
                                    {
                                        await _serviceDiscoveryCommunicator.DropServiceAsync(service.ServiceName, cancellationTokenSource);

                                        _tempData.Logs.Add(new Log()
                                        {
                                            LogText = $"Endpoint {endpoint.Name} has been deleted, url was {endpoint.Url}"
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public static Job MethodJob
        {
            get
            {
                return new Job(typeof(CheckApiServicesJob).GetMethod(nameof(CheckServicesAsync)));
            }
        }
    }
}
