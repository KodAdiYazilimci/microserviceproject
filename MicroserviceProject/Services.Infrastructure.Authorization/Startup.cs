using Communication.Http.DI;

using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Caching.Redis.DI;
using Infrastructure.Communication.Broker.DI;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Localization.DI;
using Infrastructure.Logging.Logger.RequestResponseLogger.DI;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Providers.DI;
using Infrastructure.Security.Authentication.DI;
using Infrastructure.Util.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Services.Infrastructure.Authorization.Configuration.Services;

using System.Net;

namespace Services.Infrastructure.Authorization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.RegisterCommunicators();
            services.RegisterCredentialProvider();
            services.RegisterInMemoryCaching();
            services.RegisterLocalizationPersistence();
            services.RegisterLocalizationProviders();
            services.RegisterLogger();
            services.RegisterRedisCaching();
            services.RegisterRepositories(Configuration);
            services.RegisterRouteProvider();
            services.RegisterRouteRepositories();
            services.RegisterServiceCommunicator();
            services.RegisterServices();
            services.RegisterSwagger();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<Middleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/CoreSwagger/swagger.json", "CoreSwagger");
            });
        }
    }
}
