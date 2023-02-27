using Microsoft.Extensions.Configuration;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Transaction.UnitOfWork.Sql
{
    /// <summary>
    /// Ms SQL veritabanı işlemleri transaction için iş birimi sınıfının temeli
    /// </summary>
    public abstract class BaseUnitOfWork : IUnitOfWork, IDisposable
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
        public abstract string ConnectionString { get; }

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
                {
                    sqlConnection = new SqlConnection(ConnectionString);
                }

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

                    if (sqlTransaction.SupportsSavepoints)
                    {
                        sqlTransaction.Save("SavePoint");
                    }
                }

                return sqlTransaction;
            }
        }

        /// <summary>
        /// Ms SQL veritabanı işlemleri transaction için iş birimi sınıfı
        /// </summary>
        /// <param name="configuration">Repository yapılandırmaları için configuration nesnesi</param>
        public BaseUnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
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
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    DisposeConnections();
                }

                disposed = true;
            }
        }

        private void DisposeConnections()
        {
            if (sqlConnection != null)
            {
                if (sqlConnection.State != ConnectionState.Closed)
                {
                    sqlConnection.Close();
                }
            }

            if (sqlTransaction != null)
            {
                sqlTransaction.Dispose();
            }

            sqlTransaction = null;

            if (sqlConnection != null)
            {
                sqlConnection.Dispose();
            }

            sqlConnection = null;
        }

        /// <summary>
        /// Veritabanı işlem bütünlüğünü çalıştırır
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task SaveAsync(CancellationTokenSource cancellationTokenSource)
        {
            Exception exception = null;

            try
            {
                await SqlTransaction.CommitAsync(cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                exception = ex;

                await SqlTransaction.RollbackAsync(cancellationTokenSource.Token);
            }
            finally
            {
                //if (SqlConnection.State != ConnectionState.Closed)
                //{
                //    await SqlConnection.CloseAsync();
                //}

                DisposeConnections();
            }

            if (exception != null)
            {
                throw exception;
            }
        }
    }
}
