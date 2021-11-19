﻿using Infrastructure.Transaction.Recovery;

using Microsoft.EntityFrameworkCore;

using Services.Infrastructure.Authorization.Configuration.Persistence;
using Services.Infrastructure.Authorization.Entities.EntityFramework;
using Services.Infrastructure.Authorization.Repositories;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Infrastructure.Authorization.Repositories
{
    /// <summary>
    /// Kullanıcı repository sınıfı
    /// </summary>
    public class UserRepository : BaseRepository<AuthContext, User>, IRollbackableDataAsync<int>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[USERS]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly AuthContext _context;

        /// <summary>
        /// Kullanıcı repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public UserRepository(AuthContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir kullanıcıyı siler
        /// </summary>
        /// <param name="id">Silinecek kullanıcının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        /// <summary>
        /// Bir kullanıcı kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek kullanıcının Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationTokenSource cancellationTokenSource)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (user != null)
            {
                user.GetType().GetProperty(name).SetValue(user, value);
            }
            else
            {
                throw new Exception("Kullanıcı kaydı bulunamadı");
            }

            return id;
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir kullanıcı kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak kullanıcı kaydının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                user.DeleteDate = null;
            }
            else
            {
                throw new Exception("Kullanıcı kaydı bulunamadı");
            }

            return id;
        }

        public override async Task UpdateAsync(int id, User entity, CancellationTokenSource cancellationTokenSource)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (user != null)
            {
                user.Email = entity.Email;
                user.Password = entity.Password;
            }
            else
            {
                throw new Exception("Kullanıcı kaydı bulunamadı");
            }
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <returns></returns>
        public override async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override async Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    await _context.DisposeAsync();

                    disposed = true;
                }
            }
        }
    }
}
