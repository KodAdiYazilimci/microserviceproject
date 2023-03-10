using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.DI;
using Services.Communication.Http.Broker.Department.Finance.Abstract;

namespace Services.Communication.Http.Broker.Department.Finance.DI
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
        public static IServiceCollection RegisterHttpFinanceDepartmentCommunicators(this IServiceCollection services)
        {
            services.RegisterDepartmentCommunicator();

            services.AddSingleton<IFinanceCommunicator, FinanceCommunicator>();

            return services;
        }
    }
}
