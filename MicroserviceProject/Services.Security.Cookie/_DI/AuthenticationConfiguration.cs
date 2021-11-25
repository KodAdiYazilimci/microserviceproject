
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Security.Authentication.Cookie.Abstract;
using Infrastructure.Security.Authentication.Cookie.Handlers;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Security.Cookie.Providers;

using System.Threading.Tasks;

namespace Services.Security.Cookie.DI
{
    /// <summary>
    /// Yetki DI sınıfı
    /// </summary>
    public static class AuthenticationConfiguration
    {
        /// <summary>
        /// Yetki mekanizmasını enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterCookieAuthentication(this IServiceCollection services, string loginPath, string accessDeniedPath)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.RegisterHttpAuthorizationCommunicators();
            services.RegisterInMemoryCaching();

            services.AddScoped<IIdentityProvider, DefaultIdentityProvider>();

            services
                 .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.LoginPath = loginPath;
                     options.AccessDeniedPath = accessDeniedPath;
                     options.Events.OnRedirectToAccessDenied += (options) =>
                      {
                          return Task.CompletedTask;
                      };
                 });

            services.AddScoped<CookieHandler>();

            return services;
        }
    }
}
