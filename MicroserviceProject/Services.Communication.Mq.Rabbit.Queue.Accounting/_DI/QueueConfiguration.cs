using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.Accounting.DI;
using Services.Communication.Mq.Rabbit.Queue.Accounting.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Accounting.Consumers;
using Services.Communication.Mq.Rabbit.Queue.Accounting.Publishers;

namespace Services.Communication.Mq.Rabbit.Queue.Accounting.DI
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

        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAccountingQueuePublishers(this IServiceCollection services)
        {
            RegisterAccountingQueueConfigurations(services);

            services.AddSingleton<CreateBankAccountPublisher>();

            return services;
        }

        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAccountingQueueConsumers(this IServiceCollection services)
        {
            RegisterAccountingQueueConfigurations(services);

            services.RegisterHttpAccountingDepartmentCommunicators();

            services.AddSingleton<CreateBankAccountConsumer>();

            return services;
        }
    }
}
