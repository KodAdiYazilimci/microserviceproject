using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Services.Business.Configuration.Communication.Rabbit.AA;
using MicroserviceProject.Services.Business.Departments.AA.DI;
using MicroserviceProject.Services.Business.Departments.AA.Repositories.Sql;
using MicroserviceProject.Services.Business.Departments.AA.Services;
using MicroserviceProject.Services.Business.Departments.AA.Util.Consumers.Inventory;
using MicroserviceProject.Services.Business.Util.UnitOfWork;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.AA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args).Build();

            hostBuilder.RegisterConsumers();

            hostBuilder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
