using Elasticsearch.Net;

using Infrastructure.Logging.Abstraction;
using Infrastructure.Logging.Elastic.Configuration;
using Infrastructure.Logging.Model;

using Nest;

namespace Infrastructure.Logging.Elastic.Loggers
{
    public class BulkElasticLogger<TModel> : IBulkLogger<TModel>, IDisposable where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private IElasticConfiguration elasticConfiguration;

        public BulkElasticLogger(IElasticConfiguration elasticConfiguration)
        {
            this.elasticConfiguration = elasticConfiguration;
        }

        public async Task LogAsync(List<TModel> model, CancellationTokenSource cancellationTokenSource)
        {
            using (var pool = new SingleNodeConnectionPool(new Uri(uriString: elasticConfiguration.Host)))
            {
                ConnectionSettings connection = new ConnectionSettings(pool);

                connection.BasicAuthentication(
                    username: elasticConfiguration.UserName,
                    password: elasticConfiguration.Password);

                connection.DefaultIndex(defaultIndex: elasticConfiguration.Index);

                ElasticClient client = new ElasticClient(connection);

                try
                {
                    CreateIndexResponse createIndexResponse = await client.Indices.CreateAsync(
                        index: elasticConfiguration.Index,
                        selector: index => index.Map<TModel>(x => x.AutoMap()),
                        ct: cancellationTokenSource.Token);

                    var indexDocumentResponse = await client.BulkAsync(selector: selector =>
                    {
                        return selector.CreateMany<TModel>(model, null).IndexMany<TModel>(model);
                    });
                }
                catch (Exception)
                {
                }
            }
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
                    elasticConfiguration = null;
                }

                disposed = true;
            }
        }
    }
}
