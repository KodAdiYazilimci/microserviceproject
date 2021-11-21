using Infrastructure.Security.Authentication.SignalR.Abstract;
using Infrastructure.Security.Authentication.SignalR.Handlers;
using Infrastructure.Security.Authentication.SignalR.Requirements;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Services.Security.SignalR.Providers;

namespace Services.Security.SignalR.DI
{
    /// <summary>
    /// SignalR güvenliği DI sınıfı
    /// </summary>
    public static class AuthenticationConfiguration
    {
        /// <summary>
        /// SignalR güvenliğini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <param name="policyName">Güvenlik politikasının adı</param>
        /// <returns></returns>
        public static IServiceCollection RegisterSignalRAuthentication(this IServiceCollection services, string policyName)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IIdentityProvider, DefaultIdentityProvider>();

            services.AddSingleton<IAuthorizationHandler, SignalRHandler>();

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
