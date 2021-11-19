
using Infrastructure.Security.Authentication.Cookie.Providers;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;

namespace Infrastructure.Security.Authentication.Cookie.DI
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

            services.AddScoped<SessionProvider>();

            return services;
        }
    }
}
