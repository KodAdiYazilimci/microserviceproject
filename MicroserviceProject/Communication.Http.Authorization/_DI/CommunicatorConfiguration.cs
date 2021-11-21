
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Http.Authorization.DI
{
    /// <summary>
    /// İletişimcilerin DI sınıfı
    /// </summary>
    public static class CommunicatorConfiguration
    {
        /// <summary>
        /// İletişimcileri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAuthorizationCommunicators(this IServiceCollection services)
        {
            services.AddSingleton<AuthorizationCommunicator>();
                        
            return services;
        }
    }
}
