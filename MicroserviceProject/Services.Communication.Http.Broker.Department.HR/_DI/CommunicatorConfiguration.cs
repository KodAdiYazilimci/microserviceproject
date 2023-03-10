using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.DI;
using Services.Communication.Http.Broker.Department.HR.Abstract;

namespace Services.Communication.Http.Broker.Department.HR.DI
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
        public static IServiceCollection RegisterHttpHRDepartmentCommunicators(this IServiceCollection services)
        {
            services.RegisterDepartmentCommunicator();
            services.AddSingleton<IHRCommunicator, HRCommunicator>();

            return services;
        }
    }
}
