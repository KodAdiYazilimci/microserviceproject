using Newtonsoft.Json;

using System;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Providers
{
    /// <summary>
    /// Http get isteği sağlayıcısı
    /// </summary>
    public class HttpGetProvider : BaseProvider, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Http get isteği gönderir
        /// </summary>
        /// <param name="url">Http isteği atılacak adres</param>
        /// <param name="cancellationToken">İsteğin iptal tokenı</param>
        /// <returns></returns>
        public virtual async Task<string> GetAsync(string url)
        {
            Uri requestUri = GenerateUri(url);

            HttpClient httpClient = new HttpClient();

            AppendHeaders(httpClient);

            HttpResponseMessage httpResponseMessage =
                await httpClient.GetAsync(requestUri);

            using (StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync()))
            {
                string response = await streamReader.ReadToEndAsync();

                return response;
            }
        }

        /// <summary>
        /// Http get isteği gönderir
        /// </summary>
        /// <param name="url">Http isteği atılacak adres</param>
        /// <param name="cancellationTokenSource">İsteğin iptal tokenı</param>
        /// <returns></returns>
        public virtual async Task<string> GetAsync(string url, CancellationTokenSource cancellationTokenSource)
        {
            Uri requestUri = GenerateUri(url);

            HttpClient httpClient = new HttpClient();

            AppendHeaders(httpClient);

            HttpResponseMessage httpResponseMessage =
                await httpClient.GetAsync(requestUri);

            using (StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync(cancellationTokenSource.Token)))
            {
                string response = await streamReader.ReadToEndAsync();

                return response;
            }
        }

        /// <summary>
        /// Http get isteği gönderir
        /// </summary>
        /// <typeparam name="TResult">Http isteğinden dönecek yanıtın tipi</typeparam>
        /// <param name="url">Http isteği atılacak adres</param>
        /// <param name="cancellationTokenSource">İsteğin iptal tokenı</param>
        /// <returns></returns>
        public virtual async Task<TResult> GetAsync<TResult>(string url, CancellationTokenSource cancellationTokenSource)
        {
            Uri requestUri = GenerateUri(url);

            HttpClient httpClient = new HttpClient();

            AppendHeaders(httpClient);

            HttpResponseMessage httpResponseMessage =
                await httpClient.GetAsync(requestUri, cancellationTokenSource.Token);

            using (StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync(cancellationTokenSource.Token)))
            {
                string response = await streamReader.ReadToEndAsync();

                return JsonConvert.DeserializeObject<TResult>(response);
            }
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    if (Headers != null)
                    {
                        Headers.Clear();
                        Headers = null;
                    }

                    if (Queries != null)
                    {
                        Queries.Clear();
                        Queries = null;
                    }
                }

                disposed = true;
            }

            base.Dispose();
        }
    }
}
