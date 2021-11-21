using Communication.Http.Authorization.DI;

using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Communication.Broker.DI;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Providers.DI;
using Infrastructure.Security.Authentication.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Services.Security.BasicToken.DI;
using Services.Security.SignalR.DI;
using Services.WebSockets.Reliability.Hubs;

using System.Net;

namespace Services.WebSockets.Reliability
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSignalR();
            services.AddSingleton<ErrorHub>();
            services.RegisterAuthorizationCommunicators();
            services.RegisterInMemoryCaching();
            services.RegisterBasicTokenAuthentication();
            services.RegisterCredentialProvider();
            services.RegisterRouteRepositories();
            services.RegisterRouteProvider();
            services.RegisterServiceCommunicator();
            services.RegisterCredentialProvider();
            services.RegisterSignalRAuthentication(policyName: "ErrorPolicy");

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Service is running.");
                //});

                endpoints.MapControllers();

                endpoints.MapHub<ErrorHub>("ErrorHub", options =>
                {
                    options.Transports = HttpTransportType.WebSockets;
                });
            });
        }
    }
}
