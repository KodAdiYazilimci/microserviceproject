using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.Accounting.Configuration.Validation.BankAccounts.CreateBankAccount;
using Services.Api.Business.Departments.Accounting.Configuration.Validation.Transaction;
using Services.Api.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment;
using Services.Api.Business.Departments.Accounting.Util.Validation.Transaction;

namespace Services.Api.Business.Departments.Accounting.DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<CreateBankAccountRule>();
            services.AddSingleton<CreateBankAccountValidator>();

            services.AddSingleton<CreateCurrencyRule>();
            services.AddSingleton<CreateCurrencyValidator>();

            services.AddSingleton<CreateSalaryPaymentRule>();
            services.AddSingleton<CreateSalaryPaymentValidator>();

            services.AddSingleton<RollbackTransactionRule>();
            services.AddSingleton<RollbackTransactionValidator>();

            return services;
        }
    }
}
