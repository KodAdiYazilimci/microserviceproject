﻿using MicroserviceProject.Services.Business.Departments.IT.Services;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Business.Departments.IT.DI
{
    /// <summary>
    /// İş mantığı servisleri DI sınıfı
    /// </summary>
    public static class BusinessServicesConfiguration
    {
        /// <summary>
        /// İş mantığı servislerini enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<InventoryService>();

            return services;
        }
    }
}