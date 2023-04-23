
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Register.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Communication.WebSockets.Endpoint.Security;
using Services.Security.SignalR.DI;
using Services.ServiceDiscovery.DI;
using Services.WebSockets.Security.Hubs;

using System.Collections.Generic;
using System.Net;

namespace Services.WebSockets.Security
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<TokensHub>();

            services.RegisterHttpAuthorizationCommunicators();
            services.RegisterSignalRAuthentication(policyName: "TokensPolicy");
            services.RegisterServiceRegisterers();

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

                endpoints.MapHub<TokensHub>("TokensHub", options =>
                {
                    options.Transports = HttpTransportType.WebSockets;
                });
            });

            app.RegisterService(new List<IEndpoint>()
            {
                new SendTokenNotificationEndpoint()
            });
        }
    }
}
