using Infrastructure.Communication.Http.Broker.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization;

namespace Services.Communication.Http.Broker.Department.IT.DI
{
    /// <summary>
    /// İletişimcilerin DI sınıfı
    /// </summary>
    public static class ITCommunicatorConfiguration
    {
        /// <summary>
        /// İletişimcileri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterHttpITDepartmentCommunicators(this IServiceCollection services)
        {
            services.RegisterHttpServiceCommunicator();

            services.AddSingleton<AuthorizationCommunicator>();
            services.AddSingleton<ITCommunicator>();
                        
            return services;
        }
    }
}
