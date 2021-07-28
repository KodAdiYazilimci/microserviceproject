using Communication.Http.DI;

using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Caching.Redis.DI;
using Infrastructure.Communication.Broker.DI;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Localization.DI;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Providers.DI;
using Infrastructure.Security.Authentication.BasicToken.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Services.MQ.Accounting.DI;

using System.Net;

namespace Services.MQ.Accounting
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.RegisterCommunicators();
            services.RegisterRedisCaching();
            services.RegisterRouteProvider();
            services.RegisterCredentialProvider();
            services.RegisterInMemoryCaching();
            services.RegisterServiceCommunicator();
            services.RegisterConsumers();
            services.RegisterRouteRepositories();
            services.RegisterLocalizationPersistence();
            services.RegisterLocalizationProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(handler =>
            {
                handler.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    await
                    context.Response.WriteAsync(JsonConvert.SerializeObject(new ServiceResultModel()
                    {
                        IsSuccess = false,
                        ErrorModel = new ErrorModel()
                        {
                            Description = context.Features.Get<IExceptionHandlerPathFeature>().Error.Message
                        }
                    }));
                });
            });
        }
    }
}
