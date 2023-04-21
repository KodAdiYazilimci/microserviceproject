
using Infrastructure.Communication.Http.Broker.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Gateway._DI;
using Services.Communication.Http.Broker.Gateway.Gateway.Public.Abstract;
using Services.Communication.Http.Broker.Gateway.Public.Abstract;

namespace Services.Communication.Http.Broker.Gateway.Public.DI
{
    /// <summary>
    /// İletişimcilerin DI sınıfı
    /// </summary>
    public static class PublicGatewayCommunicatorConfiguration
    {
        /// <summary>
        /// İletişimcileri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterPublicGatewayCommunicators(this IServiceCollection services)
        {
            services.RegisterHttpServiceCommunicator();

            services.RegisterGatewayCommunicators();

            services.AddSingleton<IPublicGatewayCommunicator, PublicGatewayCommunicator>();
            services.AddSingleton<IHRCommunicator, HRCommunicator>();

            return services;
        }
    }
}
