using System;
using System.Data.SqlClient;

namespace Infrastructure.Transaction.UnitOfWork.Sql
{
    /// <summary>
    /// Ms SQL veritabanı işlemleri transaction için iş birimi arayüzü
    /// </summary>
    public interface ISqlUnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Sql bütünlük yönetiminden sorumlu transaction nesnesi
        /// </summary>
        SqlTransaction SqlTransaction { get; }

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        SqlConnection SqlConnection { get; }
    }
}
