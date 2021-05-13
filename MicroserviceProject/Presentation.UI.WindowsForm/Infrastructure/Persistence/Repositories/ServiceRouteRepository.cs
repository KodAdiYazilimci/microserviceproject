
using Presentation.UI.Infrastructure.Communication.Moderator.Model;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.UI.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Servis rotaları repository sınıfı
    /// </summary>
    public class ServiceRouteRepository
    {
        /// <summary>
        /// Veritabanı bağlantı cümlesi
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Servis rotaları repository sınıfı
        /// </summary>
        /// <param name="connectionString">Veritabanı bağlantı cümlesi</param>
        public ServiceRouteRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<ServiceRouteModel>> GetServiceRoutesAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<ServiceRouteModel> routes = new List<ServiceRouteModel>();

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
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

                foreach (var route in routes)
                {
                    using (SqlCommand sqlQueryCommand = new SqlCommand(@"SELECT Q.* FROM SERVICE_ROUTES_QUERYKEYS Q
                                                                  WHERE
                                                                  Q.SERVICE_ROUTE_ID=@ROUTEID
                                                                  AND
                                                                  Q.DELETE_DATE IS NULL", sqlConnection))
                    {
                        sqlQueryCommand.Parameters.AddWithValue("@ROUTEID", route.Id);

                        using (SqlDataReader sqlQueryReader = await sqlQueryCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                        {

                            if (sqlQueryReader.HasRows)
                            {
                                while (await sqlQueryReader.ReadAsync(cancellationTokenSource.Token))
                                {
                                    RouteQuery queryKey = new RouteQuery();
                                    queryKey.Id = Convert.ToInt32(sqlQueryReader["ID"]);
                                    queryKey.CallModelId = Convert.ToInt32(sqlQueryReader["SERVICE_ROUTE_ID"]);
                                    queryKey.Key = sqlQueryReader["KEY"].ToString();

                                    route.QueryKeys.Add(queryKey);
                                }
                            }
                        }
                    }
                }

                foreach (var route in routes)
                {
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

                        using (SqlDataReader sqlAlternativeRouteReader = await sqlAlternativesCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                        {

                            if (sqlAlternativeRouteReader.HasRows)
                            {
                                while (await sqlAlternativeRouteReader.ReadAsync(cancellationTokenSource.Token))
                                {
                                    ServiceRouteModel alternativeRoute = new ServiceRouteModel();

                                    alternativeRoute.Id = Convert.ToInt32(sqlAlternativeRouteReader["ALTERNATIVE_SERVICE_ROUTE_ID"]);
                                    alternativeRoute.ServiceName = sqlAlternativeRouteReader["NAME"].ToString();
                                    alternativeRoute.CallType = sqlAlternativeRouteReader["CALLTYPE"].ToString();
                                    alternativeRoute.Endpoint = sqlAlternativeRouteReader["ENDPOINT"].ToString();
                                    alternativeRoute.QueryKeys = route.QueryKeys;

                                    if (route.AlternativeRoutes == null)
                                    {
                                        route.AlternativeRoutes = new List<ServiceRouteModel>();
                                    }

                                    route.AlternativeRoutes.Add(alternativeRoute);
                                }
                            }
                        }
                    }
                }

                foreach (var route in routes)
                {
                    if (route.AlternativeRoutes != null && route.AlternativeRoutes.Any())
                    {
                        foreach (var alternativeRoute in route.AlternativeRoutes)
                        {
                            if (alternativeRoute.QueryKeys == null)
                                alternativeRoute.QueryKeys = new List<RouteQuery>();

                            using (SqlCommand sqlAlternativeRouteQueryCommand = new SqlCommand(@"SELECT Q.* FROM SERVICE_ROUTES_QUERYKEYS Q
                                                                  WHERE
                                                                  Q.SERVICE_ROUTE_ID=@ROUTEID
                                                                  AND
                                                                  Q.DELETE_DATE IS NULL", sqlConnection))
                            {
                                sqlAlternativeRouteQueryCommand.Parameters.AddWithValue("@ROUTEID", alternativeRoute.Id);

                                using (SqlDataReader sqlAlternativeRouteQueryReader = await sqlAlternativeRouteQueryCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                                {
                                    if (sqlAlternativeRouteQueryReader.HasRows)
                                    {
                                        while (await sqlAlternativeRouteQueryReader.ReadAsync(cancellationTokenSource.Token))
                                        {
                                            RouteQuery queryKey = new RouteQuery();
                                            queryKey.Id = Convert.ToInt32(sqlAlternativeRouteQueryReader["ID"]);
                                            queryKey.CallModelId = Convert.ToInt32(sqlAlternativeRouteQueryReader["SERVICE_ROUTE_ID"]);
                                            queryKey.Key = sqlAlternativeRouteQueryReader["KEY"].ToString();

                                            alternativeRoute.QueryKeys.Add(queryKey);
                                        }
                                    }
                                }
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
                    sqlConnection.Close();
                }
            }

            if (exception != null)
            {
                throw exception;
            }

            return routes;
        }
    }
}
