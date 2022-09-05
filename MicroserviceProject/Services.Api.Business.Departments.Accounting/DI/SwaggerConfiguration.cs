using Infrastructure.Util.DI;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace Services.Api.Business.Departments.Accounting.DI
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.RegisterSwagger(
                applicationName: Environment.GetEnvironmentVariable("ApplicationName") ?? "Services.Api.Business.Departments.Accounting",
                description: "Accounting Api Service");

            return services;
        }
    }
}
