using Services.Api.Logging.Util.Logging.Loggers;

using Microsoft.Extensions.DependencyInjection;

namespace Services.Api.Logging.DI
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
