using Infrastructure.Security.Authentication.SignalR.Handlers;
using Infrastructure.Security.Authentication.SignalR.Requirements;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security.Authentication.SignalR.DI
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection RegisterSignalRSecurity(this IServiceCollection services, string policyName)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAuthorizationHandler, DefaultAuthorizationHandler>();

            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.Requirements.Add(new DefaultAuthorizationRequirement());
                });
            });

            return services;
        }
    }
}
