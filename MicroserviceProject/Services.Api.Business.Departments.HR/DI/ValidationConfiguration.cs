using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.HR.Configuration.Validation.Department.CreateDepartment;
using Services.Api.Business.Departments.HR.Configuration.Validation.Person.CreatePerson;
using Services.Api.Business.Departments.HR.Configuration.Validation.Person.CreateTitle;
using Services.Api.Business.Departments.HR.Configuration.Validation.Person.CreateWorker;
using Services.Api.Business.Departments.HR.Configuration.Validation.Transaction;
using Services.Api.Business.Departments.HR.Util.Validation.Department.CreateDepartment;
using Services.Api.Business.Departments.HR.Util.Validation.Person.CreatePerson;
using Services.Api.Business.Departments.HR.Util.Validation.Person.CreateTitle;
using Services.Api.Business.Departments.HR.Util.Validation.Person.CreateWorker;
using Services.Api.Business.Departments.HR.Util.Validation.Transaction;

namespace Services.Api.Business.Departments.HR.DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<CreateDepartmentRule>();
            services.AddSingleton<CreateDepartmentValidator>();

            services.AddSingleton<CreatePersonRule>();
            services.AddSingleton<CreatePersonValidator>();

            services.AddSingleton<CreateTitleRule>();
            services.AddSingleton<CreateTitleValidator>();

            services.AddSingleton<CreateWorkerRule>();
            services.AddSingleton<CreateWorkerValidator>();

            services.AddSingleton<RollbackTransactionRule>();
            services.AddSingleton<RollbackTransactionValidator>();

            return services;
        }
    }
}
