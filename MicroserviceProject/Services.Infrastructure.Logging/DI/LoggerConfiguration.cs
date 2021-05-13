using Services.Infrastructure.Logging.Util.Logging.Loggers;

using Microsoft.Extensions.DependencyInjection;

namespace Services.Infrastructure.Logging.DI
{
    public static class LoggerConfiguration
    {
        public static IServiceCollection RegisterLoggers(this IServiceCollection services)
        {
            services.AddSingleton<RequestResponseLogger>();

            return services;
        }
    }
}
