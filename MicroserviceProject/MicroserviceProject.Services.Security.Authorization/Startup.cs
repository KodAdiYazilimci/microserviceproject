using MicroserviceProject.Model.Communication.Basics;
using MicroserviceProject.Model.Communication.Errors;
using MicroserviceProject.Services.Security.Authorization.Business.Services;
using MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Repositories;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using System.Net;

namespace MicroserviceProject.Services.Security.Authorization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private string AuthorizationConnectionString
        {
            get
            {
                return Configuration.GetSection("Configuration").GetSection("DataSources").GetSection("Authorization").Value;
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SessionRepository>(x => new SessionRepository(AuthorizationConnectionString));
            services.AddScoped<UserRepository>(x => new UserRepository(AuthorizationConnectionString));

            services.AddScoped<SessionService>();
            services.AddScoped<UserService>();

            services.AddControllers();
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
                    context.Response.WriteAsync(JsonConvert.SerializeObject(new ServiceResult()
                    {
                        IsSuccess = false,
                        Error = new Error()
                        {
                            Description = context.Features.Get<IExceptionHandlerPathFeature>().Error.Message
                        }
                    }));
                });
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
