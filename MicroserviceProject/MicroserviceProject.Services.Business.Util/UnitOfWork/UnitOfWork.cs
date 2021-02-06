using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Util.UnitOfWork
{
    /// <summary>
    /// Ms SQL veritabanı işlemleri transaction için iş birimi sınıfı
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private SqlConnection _sqlConnection;

        /// <summary>
        /// Sql bütünlük yönetiminden sorumlu transaction nesnesi
        /// </summary>
        private SqlTransaction _sqlTransaction;

        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Ms SQL veritabanı işlemleri transaction için iş birimi sınıfı
        /// </summary>
        /// <param name="sqlConnection">Veritabanı bağlantı nesnesi</param>
        public UnitOfWork(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;

            if (sqlConnection.State != ConnectionState.Open)
            {
                _sqlConnection.Open();
            }

            _sqlTransaction = _sqlConnection.BeginTransaction();
        }

        /// <summary>
        /// Sql bütünlük yönetiminden sorumlu transaction nesnesi
        /// </summary>
        public SqlTransaction SqlTransaction
        {
            get { return _sqlTransaction; }
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
                    if (_sqlConnection.State != ConnectionState.Closed)
                    {
                        _sqlConnection.Close();
                    }

                    _sqlTransaction.Dispose();
                    _sqlTransaction = null;

                    _sqlConnection.Dispose();
                    _sqlConnection = null;
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
                await _sqlTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (_sqlConnection.State != ConnectionState.Closed)
                {
                    await _sqlConnection.CloseAsync();
                }
            }

            if (exception != null)
            {
                throw exception;
            }
        }
    }
}
