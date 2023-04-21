using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.AA.Abstract;
using Services.Communication.Http.Broker.Department.DI;

namespace Services.Communication.Http.Broker.Department.AA.DI
{
    /// <summary>
    /// İletişimcilerin DI sınıfı
    /// </summary>
    public static class AACommunicatorConfiguration
    {
        /// <summary>
        /// İletişimcileri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterHttpAADepartmentCommunicators(this IServiceCollection services)
        {
            services.RegisterDepartmentCommunicator();

            services.AddSingleton<IAACommunicator, AACommunicator>();

            return services;
        }
    }
}
