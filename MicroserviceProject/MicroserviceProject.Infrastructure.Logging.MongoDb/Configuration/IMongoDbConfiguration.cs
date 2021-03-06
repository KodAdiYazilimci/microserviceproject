using System;

namespace MicroserviceProject.Infrastructure.Logging.MongoDb.Configuration
{
    /// <summary>
    /// MongoDB ye yazılacak logların yapılandırma arayüzü
    /// </summary>
    public interface IMongoDbConfiguration
    {
        /// <summary>
        /// MongoDB bağlantı cümlesi
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Logların yazılacaği MongoDB veritabanı adı
        /// </summary>
        string DataBase { get; set; }

        /// <summary>
        /// Logların yazılacağı koleksiyonun adı
        /// </summary>
        string CollectionName { get; set; }
    }
}
