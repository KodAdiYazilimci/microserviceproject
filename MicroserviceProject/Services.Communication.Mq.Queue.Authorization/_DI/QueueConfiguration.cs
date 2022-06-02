using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Mq.Queue.Authorization.Configuration;

namespace Services.Communication.Mq.Queue.Authorization.DI
{
    /// <summary>
    /// Kuyrukların DI sınıfı
    /// </summary>
    public static class QueueConfiguration
    {
        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAuthorizationQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<InformInvalidTokenRabbitConfiguration>();

            return services;
        }      
    }
}
