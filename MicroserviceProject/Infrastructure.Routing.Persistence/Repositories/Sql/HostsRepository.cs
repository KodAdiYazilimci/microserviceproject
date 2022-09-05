
using Infrastructure.Routing.Models;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Routing.Persistence.Repositories.Sql
{
    /// <summary>
    /// Servis host adresleri repository sınıfı
    /// </summary>
    public class HostsRepository : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Veritabanı bağlantı cümlesini sağlayacak connection nesnesi
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Servis host adresleri repository sınıfı
        /// </summary>
        /// <param name="configuration">Veritabanı bağlantı cümlesini sağlayacak connection nesnesi</param>
        public HostsRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        private string ConnectionString
        {
            get
            {
                return
                    Convert.ToBoolean(
                        _configuration
                        .GetSection("Configuration")
                        .GetSection("Routing")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                    ?
                    Environment.GetEnvironmentVariable(
                        _configuration
                        .GetSection("Configuration")
                        .GetSection("Routing")["EnvironmentVariableName"])
                    :
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Routing")["DataSource"];
            }
        }

        /// <summary>
        /// Servis host adreslerini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<HostModel>> GetServiceHostsAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<HostModel> hosts = new List<HostModel>();

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            try
            {
                using (SqlCommand sqlRouteCommand =
                     new SqlCommand(@"
                                    SELECT * FROM HOSTS
                                    WHERE 
                                    DELETE_DATE IS NULL", sqlConnection))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        await sqlConnection.OpenAsync(cancellationTokenSource.Token);
                    }

                    using (SqlDataReader sqlRouteDataReader = await sqlRouteCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                    {

                        if (sqlRouteDataReader.HasRows)
                        {
                            while (await sqlRouteDataReader.ReadAsync(cancellationTokenSource.Token))
                            {
                                HostModel host = new HostModel
                                {
                                    Id = Convert.ToInt32(sqlRouteDataReader["ID"])
                                };
                                host.Name = sqlRouteDataReader["NAME"].ToString();
                                host.Host = sqlRouteDataReader["HOST"].ToString();
                                host.HostType = Convert.ToInt32(sqlRouteDataReader["HOST_TYPE"]);
                                host.Enabled = Convert.ToBoolean(sqlRouteDataReader["ENABLED"]);

                                hosts.Add(host);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (sqlConnection.State != ConnectionState.Closed)
                {
                    await sqlConnection.CloseAsync();
                }
            }

            if (exception != null)
            {
                throw exception;
            }

            return hosts;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {

                }

                disposed = true;
            }
        }
    }
}
