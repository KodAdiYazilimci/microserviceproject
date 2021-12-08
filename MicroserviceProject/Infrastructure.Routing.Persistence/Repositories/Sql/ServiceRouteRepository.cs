
using Infrastructure.Routing.Models;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Routing.Persistence.Repositories.Sql
{
    /// <summary>
    /// Servis rotaları repository sınıfı
    /// </summary>
    public class ServiceRouteRepository : IDisposable
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
        /// Servis rotaları repository sınıfı
        /// </summary>
        /// <param name="configuration">Veritabanı bağlantı cümlesini sağlayacak connection nesnesi</param>
        public ServiceRouteRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        private string ConnectionString
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Routing")["DataSource"];
            }
        }

        public async Task<List<ServiceRouteModel>> GetServiceRoutesAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<ServiceRouteModel> routes = new List<ServiceRouteModel>();

            routes = await GetRoutes(cancellationTokenSource);

            await GenerateQueryKeys(routes, cancellationTokenSource);

            await GenerateAlternativeRoutes(cancellationTokenSource, routes);

            return routes;
        }

        private async Task GenerateAlternativeRoutes(CancellationTokenSource cancellationTokenSource, List<ServiceRouteModel> routes)
        {
            foreach (var route in routes)
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                using (SqlCommand sqlAlternativesCommand = new SqlCommand(@"SELECT 
                                                                         ALT.ALTERNATIVE_SERVICE_ROUTE_ID,
                                                                         RO.[NAME],
                                                                         RO.CALLTYPE,
                                                                         RO.[ENDPOINT]
                                                                         FROM SERVICE_ROUTES_ALTERNATIVES ALT
                                                                         INNER JOIN SERVICE_ROUTES RO
                                                                         ON ALT.ALTERNATIVE_SERVICE_ROUTE_ID = RO.ID
                                                                         WHERE 
                                                                         ALT.DELETE_DATE IS NULL
                                                                         AND
                                                                         RO.DELETE_DATE IS NULL
                                                                         AND
                                                                         ALT.SERVICE_ROUTES_ID = @SERVICE_ROUTES_ID", sqlConnection))
                {

                    sqlAlternativesCommand.Parameters.AddWithValue("@SERVICE_ROUTES_ID", (((object)route.Id) ?? DBNull.Value));

                    if (sqlConnection.State != ConnectionState.Open)
                        await sqlConnection.OpenAsync();

                    using (SqlDataReader sqlAlternativeRouteReader = await sqlAlternativesCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                    {
                        if (sqlAlternativeRouteReader.HasRows)
                        {
                            while (await sqlAlternativeRouteReader.ReadAsync(cancellationTokenSource.Token))
                            {
                                ServiceRouteModel alternativeRoute = new ServiceRouteModel
                                {
                                    Id = sqlAlternativeRouteReader.GetInt32("ALTERNATIVE_SERVICE_ROUTE_ID"),
                                    ServiceName = sqlAlternativeRouteReader.GetString("NAME"),
                                    CallType = sqlAlternativeRouteReader.GetString("CALLTYPE"),
                                    Endpoint = sqlAlternativeRouteReader.GetString("ENDPOINT")
                                };

                                route.AlternativeRoute = alternativeRoute;
                            }
                        }
                    }
                }
            }

            await GenerateQueryKeys(routes, cancellationTokenSource);
        }

        private async Task<List<ServiceRouteModel>> GetRoutes(CancellationTokenSource cancellationTokenSource)
        {
            List<ServiceRouteModel> routes = new List<ServiceRouteModel>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            using (SqlCommand sqlRouteCommand =
                                 new SqlCommand(@"
                                    SELECT R.* FROM SERVICE_ROUTES R
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
                            ServiceRouteModel route = new ServiceRouteModel
                            {
                                Id = Convert.ToInt32(sqlRouteDataReader["ID"])
                            };
                            route.ServiceName = sqlRouteDataReader["NAME"].ToString();
                            route.CallType = sqlRouteDataReader["CALLTYPE"].ToString();
                            route.Endpoint = sqlRouteDataReader["ENDPOINT"].ToString();

                            routes.Add(route);
                        }
                    }
                }
            }

            return routes;
        }

        private async Task GenerateQueryKeys(List<ServiceRouteModel> routes, CancellationTokenSource cancellationTokenSource)
        {
            foreach (var route in routes)
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                using (SqlCommand sqlQueryCommand = new SqlCommand(@"SELECT Q.* FROM SERVICE_ROUTES_QUERYKEYS Q
                                                                  WHERE
                                                                  Q.SERVICE_ROUTE_ID=@ROUTEID
                                                                  AND
                                                                  Q.DELETE_DATE IS NULL", sqlConnection))
                {
                    sqlQueryCommand.Parameters.AddWithValue("@ROUTEID", route.Id);

                    if (sqlConnection.State != ConnectionState.Open)
                        await sqlConnection.OpenAsync();

                    using (SqlDataReader sqlQueryReader = await sqlQueryCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                    {
                        if (sqlQueryReader.HasRows)
                        {
                            while (await sqlQueryReader.ReadAsync(cancellationTokenSource.Token))
                            {
                                RouteQueryModel queryKey = new RouteQueryModel
                                {
                                    Id = Convert.ToInt32(sqlQueryReader["ID"]),
                                    CallModelId = Convert.ToInt32(sqlQueryReader["SERVICE_ROUTE_ID"]),
                                    Key = sqlQueryReader["KEY"].ToString()
                                };

                                route.QueryKeys.Add(queryKey);
                            }
                        }
                    }
                }
            }
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
