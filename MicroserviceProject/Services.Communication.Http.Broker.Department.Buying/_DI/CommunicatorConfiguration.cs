using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.Buying.Abstract;
using Services.Communication.Http.Broker.Department.DI;

namespace Services.Communication.Http.Broker.Department.Buying.DI
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
        public static IServiceCollection RegisterHttpBuyingDepartmentCommunicators(this IServiceCollection services)
        {
            services.RegisterDepartmentCommunicator();

            services.AddSingleton<IBuyingCommunicator, BuyingCommunicator>();

            return services;
        }
    }
}
