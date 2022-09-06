using Infrastructure.Localization.Translation.Provider.DI;
using Infrastructure.Util.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Gateway.Public.DI;
using Services.Communication.Http.Broker.Department.AA.DI;
using Services.Communication.Http.Broker.Department.Accounting.DI;
using Services.Communication.Http.Broker.Department.Buying.DI;
using Services.Communication.Http.Broker.Department.CR.DI;
using Services.Communication.Http.Broker.Department.Finance.DI;
using Services.Communication.Http.Broker.Department.HR.DI;
using Services.Communication.Http.Broker.Department.IT.DI;
using Services.Communication.Http.Broker.Department.Production.DI;
using Services.Communication.Http.Broker.Department.Selling.DI;
using Services.Communication.Http.Broker.Department.Storage.DI;
using Services.Logging.Exception.DI;
using Services.Logging.RequestResponse.DI;
using Services.Security.BasicToken.DI;
using Services.Util.Exception.Handlers;

namespace Services.Api.Gateway.Public
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
            services.AddControllers();

            services.RegisterUtilities();

            services.RegisterBasicTokenAuthentication();
            services.RegisterExceptionLogger();
            services.RegisterHttpAADepartmentCommunicators();
            services.RegisterHttpAccountingDepartmentCommunicators();
            services.RegisterHttpBuyingDepartmentCommunicators();
            services.RegisterHttpCRDepartmentCommunicators();
            services.RegisterHttpFinanceDepartmentCommunicators();
            services.RegisterHttpHRDepartmentCommunicators();
            services.RegisterHttpITDepartmentCommunicators();
            services.RegisterHttpProductionDepartmentCommunicators();
            services.RegisterHttpSellingDepartmentCommunicators();
            services.RegisterHttpStorageDepartmentCommunicators();
            services.RegisterLocalizationProviders();
            services.RegisterRequestResponseLogger();
            //services.RegisterJWTProviders();
            //services.RegisterJWT();
            services.RegisterSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.Api.Gateway.Public");

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
