﻿using Infrastructure.Communication.Http.Broker.DI;

using Microsoft.Extensions.DependencyInjection;

namespace Services.Communication.Http.Broker.Department.IT.DI
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
        public static IServiceCollection RegisterHttpITDepartmentCommunicators(this IServiceCollection services)
        {
            services.RegisterHttpServiceCommunicator();

            services.AddSingleton<ITCommunicator>();
                        
            return services;
        }
    }
}
