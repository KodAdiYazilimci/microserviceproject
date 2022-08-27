using Newtonsoft.Json;

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Providers
{
    /// <summary>
    /// Http post isteği sağlayıcısı
    /// </summary>
    public class HttpPostProvider : BaseProvider, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Http post isteği gönderir
        /// </summary>
        /// <typeparam name="TPostData">Http isteği içerisinde gönderilecek datanın tipi</typeparam>
        /// <param name="url">Http isteği atılacak adres</param>
        /// <param name="postData">Http isteği içerisinde gönderilecek data</param>
        /// <param name="cancellationTokenSource">İsteğin iptal tokenı</param>
        /// <returns></returns>
        public virtual async Task<string> PostAsync<TPostData>(string url, TPostData postData, CancellationTokenSource cancellationTokenSource)
        {
            Uri requestUri = GenerateUri(url);

            HttpClient httpClient = new HttpClient();

            AppendHeaders(httpClient);

            HttpResponseMessage httpResponseMessage =
                await httpClient.PostAsync(requestUri, new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json"), cancellationTokenSource.Token);

            using (StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync(cancellationTokenSource.Token)))
            {
                string response = await streamReader.ReadToEndAsync();

                return response;
            }
        }

        /// <summary>
        /// Http post isteği gönderir
        /// </summary>
        /// <typeparam name="TResult">Http isteğinden dönecek yanıtın tipi</typeparam>
        /// <typeparam name="TPostData">Http isteği içerisinde gönderilecek datanın tipi</typeparam>
        /// <param name="url">Http isteği atılacak adres</param>
        /// <param name="postData">Http isteği içerisinde gönderilecek data</param>
        /// <param name="cancellationTokenSource">İsteğin iptal tokenı</param>
        /// <returns></returns>
        public virtual async Task<TResult> PostAsync<TResult, TPostData>(string url, TPostData postData, CancellationTokenSource cancellationTokenSource)
        {
            Uri requestUri = GenerateUri(url);

            HttpClient httpClient = new HttpClient();

            AppendHeaders(httpClient);

            HttpResponseMessage httpResponseMessage = 
                await httpClient.PostAsync(requestUri, new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json"), cancellationTokenSource.Token);

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
        }
    }
}
