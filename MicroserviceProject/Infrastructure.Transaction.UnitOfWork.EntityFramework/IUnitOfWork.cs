
using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Transaction.UnitOfWork.EntityFramework
{
    /// <summary>
    /// Entity Framework veritabanı işlemleri transaction için iş birimi arayüzü
    /// </summary>
    /// <typeparam name="TContext">Veritabanıyla iletişim kuracak context sınıfının tipi</typeparam>
    public interface IUnitOfWork<TContext> : IAsyncDisposable where TContext : DbContext
    {
        /// <summary>
        /// Veritabanıyla iletişim kuracak context sınıfı
        /// </summary>
        TContext Context { get; set; }

        /// <summary>
        /// Veritabanı işlem bütünlüğünü çalıştırır
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        Task SaveAsync(CancellationTokenSource cancellationTokenSource);
    }
}
