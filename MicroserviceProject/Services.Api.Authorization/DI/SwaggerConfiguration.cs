using Infrastructure.Util.DI;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace Services.Api.Authorization.DI
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.RegisterSwagger(
                applicationName: Environment.GetEnvironmentVariable("ApplicationName") ?? "Services.Api.Authorization",
                description: "Authorization Api Service");

            return services;
        }
    }
}
