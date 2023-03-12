using Infrastructure.Caching.Abstraction;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;

namespace Infrastructure.Caching.InMemory
{
    /// <summary>
    /// InMemory de tutulan önbellek yönetimini sağlar
    /// </summary>
    public class InMemoryCacheDataProvider : IInMemoryCacheDataProvider, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Verilerin tutulacağı önbellek nesnesi
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// InMemory de tutulan önbellek yönetimini sağlar
        /// </summary>
        /// <param name="memoryCache">Verilerin tutulacağı önbellek nesnesi</param>
        public InMemoryCacheDataProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Önbellekteki bir listeye yeni kayıt ekler
        /// </summary>
        /// <typeparam name="T">Listenin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="value">Eklenecek kaydı nesnesi</param>
        public void AddItemToList<T>(string key, T value)
        {
            List<T> list = Get<List<T>>(key);

            if (list != null)
            {
                list.Add(value);
                Set<List<T>>(key, list);
            }
        }

        /// <summary>
        /// Önbellekten bir veriyi getirir
        /// </summary>
        /// <typeparam name="T">Getirilecek verinin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        /// <summary>
        /// Önbellekteki bir listeden kayıt siler
        /// </summary>
        /// <typeparam name="T">Listenin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="value">Silinecek öğe</param>
        public void RemoveItemOnList<T>(string key, T value)
        {
            List<T> list = Get<List<T>>(key);

            if (list != null && list.Contains(value))
            {
                list.Remove(value);
                Set<List<T>>(key, list);
            }
        }

        /// <summary>
        /// Önbellekten bir veriyi siler
        /// </summary>
        /// <param name="key">Silinecek önbelleğin anahtarı</param>
        public void RemoveObject(string key)
        {
            _memoryCache.Remove(key);
        }

        /// <summary>
        /// Önbelleğe bir kayıt ekler
        /// </summary>
        /// <typeparam name="T">Eklenecek kaydın tipi</typeparam>
        /// <param name="key">Önbelleğin anahtarı</param>
        /// <param name="value">Eklenecek nesne</param>
        public void Set<T>(string key, T value)
        {
            _memoryCache.Set<T>(key, value, new TimeSpan(hours: 0, minutes: 15, seconds: 0));
        }

        /// <summary>
        /// Önbelleğe bir kayıt ekler
        /// </summary>
        /// <typeparam name="T">Eklenecek kaydın tipi</typeparam>
        /// <param name="key">Önbelleğin anahtarı</param>
        /// <param name="value">Eklenecek nesne</param>
        /// <param name="toTime">Önbellekte tutulacak süre</param>
        public void Set<T>(string key, T value, DateTime toTime)
        {
            _memoryCache.Set<T>(key, value, toTime);
        }

        /// <summary>
        /// Önbellekten veri getirmeye çalışır
        /// </summary>
        /// <typeparam name="T">Getirilecek verinin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="value">Önbellekten getirilen veri</param>
        /// <returns></returns>
        public bool TryGetValue<T>(string key, out T value)
        {
            return _memoryCache.TryGetValue<T>(key, out value);
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
