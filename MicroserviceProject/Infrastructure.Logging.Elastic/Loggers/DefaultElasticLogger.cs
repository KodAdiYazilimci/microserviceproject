using Elasticsearch.Net;

using Infrastructure.Logging.Abstraction;
using Infrastructure.Logging.Elastic.Configuration;
using Infrastructure.Logging.Model;

using Nest;

namespace Infrastructure.Logging.Elastic.Loggers
{
    public class DefaultElasticLogger<TModel> : ILogger<TModel>, IDisposable where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private IElasticConfiguration elasticConfiguration;

        public DefaultElasticLogger(IElasticConfiguration elasticConfiguration)
        {
            this.elasticConfiguration = elasticConfiguration;
        }

        public async Task LogAsync(TModel model, CancellationTokenSource cancellationTokenSource)
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

                    IndexResponse indexDocumentResponse = await client.IndexDocumentAsync<TModel>(model, ct: cancellationTokenSource.Token);
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
