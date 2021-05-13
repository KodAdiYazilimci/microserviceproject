using Infrastructure.Security.Authentication.SignalR.Handlers;
using Infrastructure.Security.Authentication.SignalR.Requirements;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security.Authentication.SignalR.DI
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
