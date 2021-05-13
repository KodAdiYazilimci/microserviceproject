
using Infrastructure.Security.Authentication.BasicToken.Handlers;
using Infrastructure.Security.Authentication.BasicToken.Schemes;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security.Authentication.BasicToken.DI
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
        public static IServiceCollection RegisterAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(Default.DefaultScheme)
                .AddScheme<AuthenticationSchemeOptions, MasterAuthentication>(Default.DefaultScheme, null);

            return services;
        }
    }
}
