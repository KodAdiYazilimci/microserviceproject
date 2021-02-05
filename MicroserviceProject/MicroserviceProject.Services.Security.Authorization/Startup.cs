using MicroserviceProject.Model.Communication.Basics;
using MicroserviceProject.Model.Communication.Errors;
using MicroserviceProject.Services.Security.Authorization.Business.Services;
using MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Repositories;
using MicroserviceProject.Services.Security.Authorization.Util.Logging.Loggers;

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
                return Configuration.GetSection("Configuration").GetSection("Authorization").GetSection("DataSource").Value;
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<RequestResponseLogger>(x => new RequestResponseLogger(Configuration));

            services.AddScoped<SessionRepository>(x => new SessionRepository(AuthorizationConnectionString));
            services.AddScoped<UserRepository>(x => new UserRepository(AuthorizationConnectionString));

            services.AddScoped<SessionService>();
            services.AddScoped<UserService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("CoreSwagger", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "MicroserviceProject.Services.Security.Authorization Swagger",
                    Version = "1.0.0",
                    Description = "ApiGateway+UI",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Swagger Implementation Serkan Camur",
                        Url = new System.Uri("http://serkancamur.com.tr"),
                        Email = "serkan@serkancamur.com.tr"
                    },
                    TermsOfService = new System.Uri("http://swagger.io/terms/")
                });
            });

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
