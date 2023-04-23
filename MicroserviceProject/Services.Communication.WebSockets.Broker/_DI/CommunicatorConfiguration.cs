using Infrastructure.Communication.Http.Broker.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.WebSockets.Broker.Abstract;

namespace Services.Communication.WebSockets.Broker._DI
{
    public static class CommunicatorConfiguration
    {
        public static IServiceCollection RegisterSocketCommunicator(this IServiceCollection services)
        {
            services.RegisterHttpServiceCommunicator();

            services.AddSingleton<ISocketCommunicator, SocketCommunicator>();

            return services;
        }
    }
}
