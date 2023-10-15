using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.IT.Configuration.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Api.Business.Departments.IT.Configuration.Validation.Inventory.CreateInventory;
using Services.Api.Business.Departments.IT.Configuration.Validation.Inventory.InformInventoryRequest;
using Services.Api.Business.Departments.IT.Configuration.Validation.Transaction;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.CreateInventory;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.InformInventoryRequest;
using Services.Api.Business.Departments.IT.Util.Validation.Transaction;

namespace Services.Api.Business.Departments.IT.DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<CreateDefaultInventoryForNewWorkerRule>();
            services.AddSingleton<CreateDefaultInventoryForNewWorkerValidator>();

            services.AddSingleton<CreateInventoryRule>();
            services.AddSingleton<CreateInventoryValidator>();

            services.AddSingleton<InformInventoryRequestRule>();
            services.AddSingleton<InformInventoryRequestValidator>();

            services.AddSingleton<RollbackTransactionRule>();
            services.AddSingleton<RollbackTransactionValidator>();

            return services;
        }
    }
}
