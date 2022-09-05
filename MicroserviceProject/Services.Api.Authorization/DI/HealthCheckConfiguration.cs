using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Services.Diagnostics.HealthCheck.DI;


using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Services.Api.Authorization.DI
{
    public static class HealthCheckConfiguration
    {
        public static IServiceCollection RegisterSqlHealthChecking(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.RegisterSqlHealthChecking(
                connectionStrings: new List<string>()
                {
                    Convert.ToBoolean(configuration.GetSection("Persistence").GetSection("Databases").GetSection("Microservice_Security_DB")["IsSensitiveData"] ?? false.ToString())
                    &&
                    !Debugger.IsAttached
                    ?
                    Environment.GetEnvironmentVariable(configuration.GetSection("Persistence").GetSection("Databases").GetSection("Microservice_Security_DB")["EnvironmentVariableName"])
                    :
                    configuration.GetSection("Persistence").GetSection("Databases").GetSection("Microservice_Security_DB")["ConnectionString"]
                });

            return services;
        }
    }
}
