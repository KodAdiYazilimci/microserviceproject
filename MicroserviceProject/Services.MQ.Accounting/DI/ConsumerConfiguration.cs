using Services.Communication.Mq.Rabbit.Configuration.Department.Accounting;
using Services.MQ.Accounting.Util.Consumers.Inventory;

using Microsoft.Extensions.DependencyInjection;

namespace Services.MQ.Accounting.DI
{
    /// <summary>
    /// Rabbit kuyruk tüketici sınıfların DI sınıfı
    /// </summary>
    public static class ConsumerConfiguration
    {
        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterConsumers(this IServiceCollection services)
        {
            services.AddSingleton<CreateBankAccountRabbitConfiguration>();
            services.AddSingleton<CreateBankAccountConsumer>();


            return services;
        }
    }
}
