using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.CR.Configuration.Validation.Customer.CreateCustomer;
using Services.Api.Business.Departments.CR.Configuration.Validation.Transaction;
using Services.Api.Business.Departments.CR.Util.Validation.Customer.CreateCustomer;
using Services.Api.Business.Departments.CR.Util.Validation.Transaction;

namespace Services.Api.Business.Departments.CR.DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<CreateCustomerRule>();
            services.AddSingleton<CreateCustomerValidator>();

            services.AddSingleton<RollbackTransactionRule>();
            services.AddSingleton<RollbackTransactionValidator>();

            return services;
        }
    }
}
