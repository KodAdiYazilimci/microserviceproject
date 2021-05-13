using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Communication.WebSockets.DI
{
    public static class SocketConfiguration
    {
        public static IServiceCollection RegisterSocketConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<SocketListener>();

            return services;
        }
    }
}
