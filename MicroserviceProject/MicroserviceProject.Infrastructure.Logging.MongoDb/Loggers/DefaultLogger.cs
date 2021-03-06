using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.Model;
using MicroserviceProject.Infrastructure.Logging.MongoDb.Configuration;

using MongoDB.Driver;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.MongoDb.Loggers
{
    /// <summary>
    /// Herhangi bir TModel tipindeki logları MongoDB veritabanına yazan sınıf
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class DefaultLogger<TModel> : ILogger<TModel>, IDisposable where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        protected bool Disposed = false;

        /// <summary>
        /// Logların yazılacağı MongoDB veritabanına ait yapılandırma
        /// </summary>
        private IMongoDbConfiguration _mongoDbConfiguration;

        /// <summary>
        /// Herhangi bir TModel tipindeki logları MongoDB veritabanına yazan sınıf
        /// </summary>
        /// <param name="mongoDbConfiguration">Logların yazılacağı MongoDB veritabanına ait yapılandırma</param>
        public DefaultLogger(IMongoDbConfiguration mongoDbConfiguration)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
        }

        /// <summary>
        /// MongoDB ye log yazar
        /// </summary>
        /// <param name="model">Yazılacak logun modeli</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task LogAsync(TModel model, CancellationToken cancellationToken)
        {
            MongoClient client = new MongoClient(_mongoDbConfiguration.ConnectionString);
            IMongoDatabase database = client.GetDatabase(_mongoDbConfiguration.DataBase);

            var collection = database.GetCollection<TModel>(_mongoDbConfiguration.CollectionName);

            await collection.InsertOneAsync(
                document: model,
                options: new InsertOneOptions()
                {
                    BypassDocumentValidation = false
                },
                cancellationToken: cancellationToken);
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
                if (!Disposed)
                {
                    _mongoDbConfiguration = null;
                }

                Disposed = true;
            }
        }
    }
}
