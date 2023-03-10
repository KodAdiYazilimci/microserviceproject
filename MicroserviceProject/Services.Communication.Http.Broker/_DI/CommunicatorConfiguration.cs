using Infrastructure.Communication.Http.Broker.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Abstract;

namespace Services.Communication.Http.Broker.DI
{
    public static class CommunicatorConfiguration
    {
        public static IServiceCollection RegisterDefaultCommunicator(this IServiceCollection services)
        {
            services.RegisterHttpServiceCommunicator();

            services.AddSingleton<ICommunicator, DefaultCommunicator>();

            return services;
        }
    }
}
