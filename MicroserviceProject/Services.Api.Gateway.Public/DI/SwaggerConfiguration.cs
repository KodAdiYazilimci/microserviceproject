using Infrastructure.Util.DI;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace Services.Api.Gateway.Public.DI
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.RegisterSwagger(
                applicationName: Environment.GetEnvironmentVariable("ApplicationName") ?? "Services.Api.Gateway.Public",
                description: "Public Gateway API");

            return services;
        }
    }
}
