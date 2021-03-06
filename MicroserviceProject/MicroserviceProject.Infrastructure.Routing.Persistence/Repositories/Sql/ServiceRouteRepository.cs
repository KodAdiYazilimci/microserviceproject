﻿using MicroserviceProject.Infrastructure.Routing.Model;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Routing.Persistence.Repositories.Sql
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
                    .GetSection("Routing")
                    .GetSection("DataSource").Value;
            }
        }

        public async Task<List<ServiceRouteModel>> GetServiceRoutesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<ServiceRouteModel> routes = new List<ServiceRouteModel>();

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            try
            {
                SqlCommand sqlRouteCommand =
                    new SqlCommand(@"
                                    SELECT R.* FROM SERVICE_ROUTES R
                                    WHERE 
                                    R.DELETE_DATE IS NULL", sqlConnection);

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

                        ServiceRouteModel route = new ServiceRouteModel
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
                                                                  Q.DELETE_DATE IS NULL", sqlConnection);

                    sqlQueryCommand.Parameters.AddWithValue("@ROUTEID", route.Id);

                    SqlDataReader sqlQueryReader = await sqlQueryCommand.ExecuteReaderAsync(cancellationToken);

                    if (sqlQueryReader.HasRows)
                    {
                        while (await sqlQueryReader.ReadAsync(cancellationToken))
                        {
                            cancellationToken.ThrowIfCancellationRequested();

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

                foreach (var route in routes)
                {
                    SqlCommand sqlAlternativesCommand = new SqlCommand(@"SELECT 
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
                                                                         ALT.SERVICE_ROUTES_ID = @SERVICE_ROUTES_ID", sqlConnection);

                    sqlAlternativesCommand.Parameters.AddWithValue("@SERVICE_ROUTES_ID", (((object)route.Id) ?? DBNull.Value));

                    SqlDataReader sqlAlternativeRouteReader = await sqlAlternativesCommand.ExecuteReaderAsync(cancellationToken);

                    if (sqlAlternativeRouteReader.HasRows)
                    {
                        while (await sqlAlternativeRouteReader.ReadAsync(cancellationToken))
                        {
                            ServiceRouteModel alternativeRoute = new ServiceRouteModel
                            {
                                Id = sqlAlternativeRouteReader.GetInt32("ALTERNATIVE_SERVICE_ROUTE_ID"),
                                ServiceName = sqlAlternativeRouteReader.GetString("NAME"),
                                CallType = sqlAlternativeRouteReader.GetString("CALLTYPE"),
                                Endpoint = sqlAlternativeRouteReader.GetString("ENDPOINT")
                            };

                            if (route.AlternativeRoutes == null)
                            {
                                route.AlternativeRoutes = new List<ServiceRouteModel>();
                            }

                            route.AlternativeRoutes.Add(alternativeRoute);
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
                                alternativeRoute.QueryKeys = new List<RouteQueryModel>();

                            SqlCommand sqlAlternativeRouteQueryCommand = new SqlCommand(@"SELECT Q.* FROM SERVICE_ROUTES_QUERYKEYS Q
                                                                  WHERE
                                                                  Q.SERVICE_ROUTE_ID=@ROUTEID
                                                                  AND
                                                                  Q.DELETE_DATE IS NULL", sqlConnection);

                            sqlAlternativeRouteQueryCommand.Parameters.AddWithValue("@ROUTEID", alternativeRoute.Id);

                            SqlDataReader sqlAlternativeRouteQueryReader = await sqlAlternativeRouteQueryCommand.ExecuteReaderAsync(cancellationToken);

                            if (sqlAlternativeRouteQueryReader.HasRows)
                            {
                                while (await sqlAlternativeRouteQueryReader.ReadAsync(cancellationToken))
                                {
                                    cancellationToken.ThrowIfCancellationRequested();

                                    RouteQueryModel queryKey = new RouteQueryModel
                                    {
                                        Id = Convert.ToInt32(sqlAlternativeRouteQueryReader["ID"]),
                                        CallModelId = Convert.ToInt32(sqlAlternativeRouteQueryReader["SERVICE_ROUTE_ID"]),
                                        Key = sqlAlternativeRouteQueryReader["KEY"].ToString()
                                    };

                                    alternativeRoute.QueryKeys.Add(queryKey);
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