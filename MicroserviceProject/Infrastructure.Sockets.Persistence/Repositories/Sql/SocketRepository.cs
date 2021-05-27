
using Infrastructure.Sockets.Models;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Sockets.Persistence.Repositories.Sql
{
    /// <summary>
    /// Web soketler repository sınıfı
    /// </summary>
    public class SocketRepository : IDisposable
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
        /// Web soketler repository sınıfı
        /// </summary>
        /// <param name="configuration">Veritabanı bağlantı cümlesini sağlayacak connection nesnesi</param>
        public SocketRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Soket bilgisi getirilirken kullanılacak SQL connection cümlesi
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("WebSockets")["DataSource"];
            }
        }

        /// <summary>
        /// Web soketlerini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<SocketModel>> GetSocketsAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<SocketModel> routes = new List<SocketModel>();

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            try
            {
                using (SqlCommand sqlRouteCommand =
                     new SqlCommand(@"
                                    SELECT R.* FROM WEBSOCKETS R
                                    WHERE 
                                    R.DELETE_DATE IS NULL", sqlConnection))
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
                                SocketModel socket = new SocketModel
                                {
                                    Id = Convert.ToInt32(sqlRouteDataReader["ID"])
                                };
                                socket.Name = sqlRouteDataReader["NAME"].ToString();
                                socket.Endpoint = sqlRouteDataReader["ENDPOINT"].ToString();
                                socket.Method = sqlRouteDataReader["METHOD"].ToString();

                                routes.Add(socket);
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

            return routes;
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
