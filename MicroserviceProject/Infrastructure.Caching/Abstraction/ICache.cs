using System;

namespace Infrastructure.Caching.Abstraction
{
    /// <summary>
    /// Önbellek sağlayıcılarının uygulayacağı arayüz
    /// </summary>
    public interface ICacheable
    {
        /// <summary>
        /// Önbellekten bir veriyi getirir
        /// </summary>
        /// <typeparam name="T">Getirilecek verinin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <returns></returns>
        T GetObject<T>(string key);

        /// <summary>
        /// Önbellekteki bir listeye yeni kayıt ekler
        /// </summary>
        /// <typeparam name="T">Listenin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="item">Eklenecek kaydı nesnesi</param>
        void AddItemToList<T>(string key, T item);

        /// <summary>
        /// Önbellekteki bir listeden kayıt siler
        /// </summary>
        /// <typeparam name="T">Listenin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="item">Silinecek öğe</param>
        void RemoveItemOnList<T>(string key, T item);

        /// <summary>
        /// Önbellekten bir veriyi siler
        /// </summary>
        /// <param name="key">Silinecek önbelleğin anahtarı</param>
        void RemoveObject(string key);

        /// <summary>
        /// Önbelleğe bir kayıt ekler
        /// </summary>
        /// <typeparam name="T">Eklenecek kaydın tipi</typeparam>
        /// <param name="key">Önbelleğin anahtarı</param>
        /// <param name="item">Eklenecek nesne</param>
        void Set<T>(string key, T item);

        /// <summary>
        /// Önbelleğe bir kayıt ekler
        /// </summary>
        /// <typeparam name="T">Eklenecek kaydın tipi</typeparam>
        /// <param name="key">Önbelleğin anahtarı</param>
        /// <param name="item">Eklenecek nesne</param>
        /// <param name="toTime">Önbellekte tutulacak süre</param>
        void Set<T>(string key, T item, TimeSpan toTime);

        /// <summary>
        /// Önbellekten veri getirmeye çalışır
        /// </summary>
        /// <typeparam name="T">Getirilecek verinin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="item">Önbellekten getirilen veri</param>
        /// <returns></returns>
        bool TryGetValue<T>(string key, out T item);
    }
}
