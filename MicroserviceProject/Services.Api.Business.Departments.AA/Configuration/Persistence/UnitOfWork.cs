using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

using System;
using System.Diagnostics;

namespace Services.Api.Business.Departments.AA.Configuration.Persistence
{
    /// <summary>
    /// Ms SQL veritabanı işlemleri transaction için iş birimi sınıfı
    /// </summary>
    public class UnitOfWork : SqlUnitOfWork, IDisposable
    {
        private readonly IConfiguration _configuration;

        public UnitOfWork(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public override string ConnectionString
        {
            get
            {
                return
                    Convert.ToBoolean(_configuration.GetSection("Persistence").GetSection("Databases").GetSection("Microservice_AA_DB")["IsSensitiveData"] ?? false.ToString())
                    &&
                    !Debugger.IsAttached
                    ?
                    Environment.GetEnvironmentVariable(_configuration.GetSection("Persistence").GetSection("Databases").GetSection("Microservice_AA_DB")["EnvironmentVariableName"])
                    :
                    _configuration
                    .GetSection("Persistence")
                    .GetSection("Databases")
                    .GetSection("Microservice_AA_DB")["ConnectionString"];
            }
        }
    }
}
