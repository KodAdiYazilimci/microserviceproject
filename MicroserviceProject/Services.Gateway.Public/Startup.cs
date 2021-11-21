
using Communication.Http.Authorization.DI;
using Communication.Http.Department.DI;

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

using Services.Gateway.Public.DI;
using Services.Security.BasicToken.DI;

using System.Net;

namespace Services.Gateway.Public
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddControllers();
            services.RegisterBasicTokenAuthentication();
            services.AddMemoryCache();
            services.RegisterRedisCaching();
            services.RegisterAuthorizationCommunicators();
            services.RegisterDepartmentCommunicators();
            services.RegisterCredentialProvider();
            services.RegisterInMemoryCaching();
            services.RegisterLocalizationPersistence();
            services.RegisterLocalizationProviders();
            services.RegisterLogger();
            services.RegisterRouteProvider();
            services.RegisterRouteRepositories();
            services.RegisterServiceCommunicator();
            services.RegisterServices();
            //services.RegisterJWTProviders();
            //services.RegisterJWT();
            services.RegisterSwagger();
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

            app.UseMiddleware<Middleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

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
