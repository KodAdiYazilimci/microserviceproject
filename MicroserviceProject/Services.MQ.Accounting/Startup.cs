using Infrastructure.Communication.Http.Models;
using Infrastructure.Localization.Translation.Provider.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Communication.Http.Broker.Department.Accounting.DI;
using Services.Communication.Mq.Queue.Accounting.DI;
using Services.Communication.Mq.Queue.Accounting.Rabbit.DI;
using Services.Logging.Exception;
using Services.Logging.Exception.Configuration;
using Services.Logging.Exception.DI;
using Services.Util.Exception.Handlers;

using System;
using System.Net;

namespace Services.MQ.Accounting
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterAccountingQueueConfigurations();
            services.RegisterAccountingQueueConsumers();
            services.RegisterExceptionLogger();
            services.RegisterHttpAuthorizationCommunicators();
            services.RegisterHttpAccountingDepartmentCommunicators();
            services.RegisterLocalizationProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.MQ.Accounting");
        }
    }
}
