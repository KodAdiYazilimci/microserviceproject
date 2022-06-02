using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Mq.Queue.Accounting.Configuration;

namespace Services.Communication.Mq.Queue.Accounting.DI
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
        public static IServiceCollection RegisterAccountingQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<CreateBankAccountRabbitConfiguration>();

            return services;
        }
    }
}
