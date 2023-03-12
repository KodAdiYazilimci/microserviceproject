using System;

namespace Infrastructure.Caching.Abstraction
{
    /// <summary>
    /// Önbellek sağlayıcılarının uygulayacağı arayüz
    /// </summary>
    public interface ICacheProvider : IDisposable
    {
        /// <summary>
        /// Önbellekten bir veriyi getirir
        /// </summary>
        /// <typeparam name="T">Getirilecek verinin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Önbellekteki bir listeye yeni kayıt ekler
        /// </summary>
        /// <typeparam name="T">Listenin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="value">Eklenecek kaydı nesnesi</param>
        void AddItemToList<T>(string key, T value);

        /// <summary>
        /// Önbellekteki bir listeden kayıt siler
        /// </summary>
        /// <typeparam name="T">Listenin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="item">Silinecek öğe</param>
        void RemoveItemOnList<T>(string key, T value);

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
        /// <param name="value">Eklenecek nesne</param>
        void Set<T>(string key, T value);

        /// <summary>
        /// Önbelleğe bir kayıt ekler
        /// </summary>
        /// <typeparam name="T">Eklenecek kaydın tipi</typeparam>
        /// <param name="key">Önbelleğin anahtarı</param>
        /// <param name="value">Eklenecek nesne</param>
        /// <param name="toTime">Önbellekte tutulacak süre</param>
        void Set<T>(string key, T value, DateTime toTime);

        /// <summary>
        /// Önbellekten veri getirmeye çalışır
        /// </summary>
        /// <typeparam name="T">Getirilecek verinin tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="value">Önbellekten getirilen veri</param>
        /// <returns></returns>
        bool TryGetValue<T>(string key, out T value);
    }
}
