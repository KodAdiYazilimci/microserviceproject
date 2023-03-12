
using Microsoft.EntityFrameworkCore;

using System;

namespace Infrastructure.Transaction.UnitOfWork.EntityFramework
{
    /// <summary>
    /// Entity Framework veritabanı işlemleri transaction için iş birimi arayüzü
    /// </summary>
    /// <typeparam name="TContext">Veritabanıyla iletişim kuracak context sınıfının tipi</typeparam>
    public interface IEfUnitOfWork<TContext> : IUnitOfWork, IAsyncDisposable where TContext : DbContext
    {
        /// <summary>
        /// Veritabanıyla iletişim kuracak context sınıfı
        /// </summary>
        TContext Context { get; set; }
    }
}
