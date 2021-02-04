using MicroserviceProject.Model.Communication.Moderator;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.ServiceRoutes.Sql.Repositories
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

        public async Task<List<ServiceRoute>> GetServiceRoutesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<ServiceRoute> routes = new List<ServiceRoute>();

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                SqlCommand sqlRouteCommand =
                    new SqlCommand(@"
                                    SELECT R.* FROM SERVICE_ROUTES R
                                    WHERE 
                                    R.DELETEDATE IS NULL", sqlConnection);

                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync(cancellationToken);
                }

                SqlDataReader sqlRouteDataReader = await sqlRouteCommand.ExecuteReaderAsync(cancellationToken);

                if (sqlRouteDataReader.HasRows)
                {
                    while (await sqlRouteDataReader.ReadAsync(cancellationToken))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        ServiceRoute route = new ServiceRoute
                        {
                            Id = Convert.ToInt32(sqlRouteDataReader["ID"])
                        };
                        route.ServiceName = sqlRouteDataReader["NAME"].ToString();
                        route.CallType = sqlRouteDataReader["CALLTYPE"].ToString();
                        route.Endpoint = sqlRouteDataReader["ENDPOINT"].ToString();

                        routes.Add(route);
                    }

                    await sqlRouteDataReader.CloseAsync();
                    await sqlRouteDataReader.DisposeAsync();
                    await sqlRouteCommand.DisposeAsync();
                }

                foreach (var route in routes)
                {
                    SqlCommand sqlQueryCommand = new SqlCommand(@"SELECT Q.* FROM SERVICE_ROUTES_QUERYKEYS Q
                                                                  WHERE
                                                                  Q.SERVICE_ROUTE_ID=@ROUTEID
                                                                  AND
                                                                  Q.DELETEDATE IS NULL", sqlConnection);

                    sqlQueryCommand.Parameters.AddWithValue("@ROUTEID", route.Id);

                    SqlDataReader sqlQueryReader = await sqlQueryCommand.ExecuteReaderAsync(cancellationToken);

                    if (sqlQueryReader.HasRows)
                    {
                        while (await sqlQueryReader.ReadAsync(cancellationToken))
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            RouteQuery queryKey = new RouteQuery();
                            queryKey.Id = Convert.ToInt32(sqlQueryReader["ID"]);
                            queryKey.CallModelId = Convert.ToInt32(sqlQueryReader["SERVICE_ROUTE_ID"]);
                            queryKey.Key = sqlQueryReader["KEY"].ToString();

                            route.QueryKeys.Add(queryKey);
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
    }
}
