using Infrastructure.Caching.Redis.DI;
using Infrastructure.Communication.Model.Basics;
using Infrastructure.Communication.Model.Errors;
using Infrastructure.Communication.Moderator.DI;
using Infrastructure.Localization.DI;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Providers.DI;
using Infrastructure.Security.Authentication.BasicToken.DI;
using Services.MQ.Buying.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using System.Net;
using Infrastructure.Caching.InMemory.DI;

namespace Services.MQ.Buying
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
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
