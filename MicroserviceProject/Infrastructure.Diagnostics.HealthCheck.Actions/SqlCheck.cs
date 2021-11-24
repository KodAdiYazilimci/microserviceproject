using Microsoft.Extensions.Diagnostics.HealthChecks;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Diagnostics.HealthCheck.Actions
{
    /// <summary>
    /// Veritabanı bağlantılarının sağlıklı çalıştığını kontrol eden sınıf
    /// </summary>
    public class SqlCheck : IHealthCheck
    {
        /// <summary>
        /// Denetlenecek veritabanı bağlantı cümleleleri
        /// </summary>
        private readonly List<string> _connectionStrings;

        /// <summary>
        /// Veritabanı bağlantılarının sağlıklı çalıştığını kontrol eden sınıf
        /// </summary>
        /// <param name="connectionStrings">Denetlenecek veritabanı bağlantı cümleleleri</param>
        public SqlCheck(List<string> connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        /// <summary>
        /// Veritabanı bağlantılarının sağlıklı çalıştığını kontrol eder
        /// </summary>
        /// <param name="context">HealthCheckContext nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            foreach (var connectionString in _connectionStrings)
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        using (SqlCommand sqlCommand = new SqlCommand("SELECT 1", sqlConnection))
                        {
                            if (sqlConnection.State != System.Data.ConnectionState.Open)
                            {
                                await sqlConnection.OpenAsync(cancellationToken);
                            }

                            int result = await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new HealthCheckResult(HealthStatus.Unhealthy, exception: ex, description: "SQL is unhealthly");
                    }

                }
            }

            return new HealthCheckResult(HealthStatus.Healthy, "SQL is healthly");
        }
    }
}
