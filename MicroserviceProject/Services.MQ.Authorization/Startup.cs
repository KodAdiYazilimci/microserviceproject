using Infrastructure.Localization.Translation.Provider.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Communication.Http.Broker.Department.AA.DI;
using Services.Communication.Http.Broker.Department.Accounting.DI;
using Services.Communication.Http.Broker.Department.Buying.DI;
using Services.Communication.Http.Broker.Department.CR.DI;
using Services.Communication.Http.Broker.Department.Finance.DI;
using Services.Communication.Http.Broker.Department.HR.DI;
using Services.Communication.Http.Broker.Department.IT.DI;
using Services.Communication.Http.Broker.Department.Production.DI;
using Services.Communication.Http.Broker.Department.Selling.DI;
using Services.Communication.Http.Broker.Department.Storage.DI;
using Services.Communication.Mq.Queue.Authorization.DI;
using Services.Communication.Mq.Queue.Authorization.Rabbit.DI;
using Services.Logging.Exception.DI;
using Services.Util.Exception.Handlers;

namespace Services.MQ.Authorization
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterAuthorizationQueueConfigurations();
            services.RegisterAuthorizationQueueConsumers();
            services.RegisterExceptionLogger();
            services.RegisterHttpAuthorizationCommunicators();
            services.RegisterHttpAADepartmentCommunicators();
            services.RegisterHttpAccountingDepartmentCommunicators();
            services.RegisterHttpBuyingDepartmentCommunicators();
            services.RegisterHttpCRDepartmentCommunicators();
            services.RegisterHttpFinanceDepartmentCommunicators();
            services.RegisterHttpHRDepartmentCommunicators();
            services.RegisterHttpITDepartmentCommunicators();
            services.RegisterHttpProductionDepartmentCommunicators();
            services.RegisterHttpSellingDepartmentCommunicators();
            services.RegisterHttpStorageDepartmentCommunicators();
            services.RegisterLocalizationProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.MQ.Authorization");
        }
    }
}
