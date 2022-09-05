using Infrastructure.Util.DI;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace Services.Api.Business.Departments.AA.DI
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.RegisterSwagger(
                applicationName: Environment.GetEnvironmentVariable("ApplicationName") ?? "Services.Api.Business.Departments.AA",
                description: "AA Api Service");

            return services;
        }
    }
}
