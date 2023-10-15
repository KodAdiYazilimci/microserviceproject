using Microsoft.Extensions.DependencyInjection;

using Services.Business.Departments.Finance.Configuration.Validation.Cost.CreateCost;
using Services.Business.Departments.Finance.Configuration.Validation.Cost.DecideCost;
using Services.Business.Departments.Finance.Configuration.Validation.Request.CreateProductionRequest;
using Services.Business.Departments.Finance.Configuration.Validation.Request.DecideProductionRequest;
using Services.Business.Departments.Finance.Configuration.Validation.Transaction;
using Services.Business.Departments.Finance.Util.Validation.Cost.CreateCost;
using Services.Business.Departments.Finance.Util.Validation.Cost.DecideCost;
using Services.Business.Departments.Finance.Util.Validation.Request.CreateProductionRequest;
using Services.Business.Departments.Finance.Util.Validation.Transaction;

namespace Services.Api.Business.Departments.Finance.DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<CreateCostRule>();
            services.AddSingleton<CreateCostValidator>();

            services.AddSingleton<DecideCostRule>();
            services.AddSingleton<DecideCostValidator>();

            services.AddSingleton<CreateProductionRequestRule>();
            services.AddSingleton<CreateProductionRequestValidator>();

            services.AddSingleton<DecideProductionRequestRule>();
            services.AddSingleton<DecideProductionRequestValidator>();

            services.AddSingleton<RollbackTransactionRule>();
            services.AddSingleton<RollbackTransactionValidator>();

            return services;
        }
    }
}
