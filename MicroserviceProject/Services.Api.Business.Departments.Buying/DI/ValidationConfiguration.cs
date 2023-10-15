using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.Buying.Configuration.Validation.Request.CreateInventoryRequest;
using Services.Api.Business.Departments.Buying.Configuration.Validation.Request.ValidateCostInventory;
using Services.Api.Business.Departments.Buying.Configuration.Validation.Transaction;
using Services.Api.Business.Departments.Buying.Util.Validation.Request.CreateInventoryRequest;
using Services.Api.Business.Departments.Buying.Util.Validation.Request.ValidateCostInventory;
using Services.Api.Business.Departments.Buying.Util.Validation.Transaction;

namespace Services.Api.Business.Departments.Buying.DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<CreateInventoryRequestRule>();
            services.AddSingleton<CreateInventoryRequestValidator>();

            services.AddSingleton<ValidateCostInventoryRule>();
            services.AddSingleton<ValidateCostInventoryValidator>();

            services.AddSingleton<RollbackTransactionRule>();
            services.AddSingleton<RollbackTransactionValidator>();

            return services;
        }
    }
}
