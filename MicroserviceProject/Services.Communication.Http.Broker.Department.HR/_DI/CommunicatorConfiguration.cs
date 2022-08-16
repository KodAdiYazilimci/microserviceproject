﻿using Infrastructure.Communication.Http.Broker.DI;

using Microsoft.Extensions.DependencyInjection;

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
            services.RegisterHttpServiceCommunicator();

            services.AddSingleton<HRCommunicator>();
                        
            return services;
        }
    }
}