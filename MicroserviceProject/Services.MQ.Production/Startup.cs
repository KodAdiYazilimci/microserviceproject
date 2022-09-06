using Infrastructure.Localization.Translation.Provider.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Communication.Http.Broker.Department.Production.DI;
using Services.Communication.Mq.Queue.Production.DI;
using Services.Communication.Mq.Queue.Production.Rabbit.DI;
using Services.Logging.Exception.DI;
using Services.Util.Exception.Handlers;

namespace Services.MQ.Production
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterExceptionLogger();
            services.RegisterHttpAuthorizationCommunicators();
            services.RegisterProductionQueueConfigurations();
            services.RegisterProductionQueueConsumers();
            services.RegisterHttpProductionDepartmentCommunicators();
            services.RegisterLocalizationProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.MQ.Production");
        }
    }
}
