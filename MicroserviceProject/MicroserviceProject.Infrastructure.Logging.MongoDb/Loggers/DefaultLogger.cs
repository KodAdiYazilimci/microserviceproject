using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.Model;
using MicroserviceProject.Infrastructure.Logging.MongoDb.Configuration;

using MongoDB.Driver;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.MongoDb.Loggers
{
    /// <summary>
    /// Herhangi bir TModel tipindeki logları MongoDB veritabanına yazan sınıf
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class DefaultLogger<TModel> : ILogger<TModel> where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Logların yazılacağı MongoDB veritabanına ait yapılandırma
        /// </summary>
        private readonly IMongoDbConfiguration _mongoDbConfiguration;

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
    }
}
