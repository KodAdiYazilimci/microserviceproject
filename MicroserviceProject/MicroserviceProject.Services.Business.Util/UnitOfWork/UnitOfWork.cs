using Microsoft.Extensions.Configuration;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Util.UnitOfWork
{
    /// <summary>
    /// Ms SQL veritabanı işlemleri transaction için iş birimi sınıfı
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repository yapılandırmaları için configuration nesnesi
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Repository sınıflarda kullanılacak veritabanı bağlantı cümlesi
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return _configuration
                    .GetSection("Persistence")
                    .GetSection("DataSource").Value;
            }
        }

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private SqlConnection sqlConnection;

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        public SqlConnection SqlConnection
        {
            get
            {
                if (sqlConnection == null)
                    sqlConnection = new SqlConnection(ConnectionString);

                return sqlConnection;
            }
        }

        /// <summary>
        /// Sql bütünlük yönetiminden sorumlu transaction nesnesi
        /// </summary>
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Sql bütünlük yönetiminden sorumlu transaction nesnesi
        /// </summary>
        public SqlTransaction SqlTransaction
        {
            get
            {
                if (sqlTransaction == null)
                {
                    if (SqlConnection.State != ConnectionState.Open)
                    {
                        SqlConnection.Open();
                    }

                    sqlTransaction = SqlConnection.BeginTransaction();
                }

                return sqlTransaction;
            }
        }

        /// <summary>
        /// Ms SQL veritabanı işlemleri transaction için iş birimi sınıfı
        /// </summary>
        /// <param name="configuration">Repository yapılandırmaları için configuration nesnesi</param>
        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    if (SqlConnection.State != ConnectionState.Closed)
                    {
                        SqlConnection.Close();
                    }

                    SqlTransaction.Dispose();

                    SqlConnection.Dispose();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Veritabanı işlem bütünlüğünü çalıştırır
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            Exception exception = null;

            try
            {
                await SqlTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (SqlConnection.State != ConnectionState.Closed)
                {
                    await SqlConnection.CloseAsync();
                }
            }

            if (exception != null)
            {
                throw exception;
            }
        }
    }
}
