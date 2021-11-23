
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Security.Authentication.BasicToken.Abstracts;
using Infrastructure.Security.Authentication.BasicToken.Handlers;
using Infrastructure.Security.Authentication.BasicToken.Schemes;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Security.BasicToken.Providers;

namespace Services.Security.BasicToken.DI
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
        public static IServiceCollection RegisterBasicTokenAuthentication(this IServiceCollection services)
        {
            services.RegisterHttpAuthorizationCommunicators();
            services.RegisterInMemoryCaching();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IIdentityProvider, DefaultIdentityProvider>();

            services
                .AddAuthentication(Default.DefaultScheme)
                .AddScheme<AuthenticationSchemeOptions, TokenHandler>(Default.DefaultScheme, null);

            return services;
        }
    }
}
