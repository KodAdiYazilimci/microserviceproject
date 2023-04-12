using Infrastructure.Util.DI;

namespace Services.Api.ServiceDiscovery.DI
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.RegisterSwagger(
                applicationName: Environment.GetEnvironmentVariable("ApplicationName") ?? "Services.Api.ServiceDiscovery",
                description: "ServiceDiscovery Api Service");

            return services;
        }
    }
}
