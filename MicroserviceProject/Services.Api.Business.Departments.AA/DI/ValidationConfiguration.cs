using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.CreateInventory;
using Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.InformInventoryRequest;
using Services.Api.Business.Departments.AA.Configuration.Validation.Transaction;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateInventory;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.InformInventoryRequest;
using Services.Api.Business.Departments.AA.Util.Validation.Transaction;

namespace Services.Api.Business.Departments.AA.DI
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
