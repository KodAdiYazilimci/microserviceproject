
using Infrastructure.Security.Authentication.Cookie.Providers;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

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
        public static IServiceCollection RegisterCookieAuthentication(this IServiceCollection services, string loginPath)
        {
            services
                 .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.LoginPath = loginPath;
                 });

            services.AddScoped<SessionProvider>();

            return services;
        }
    }
}
