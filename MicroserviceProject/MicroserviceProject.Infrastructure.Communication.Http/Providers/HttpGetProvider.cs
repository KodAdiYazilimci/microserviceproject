using Newtonsoft.Json;

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Communication.Http.Providers
{
    /// <summary>
    /// Http get isteği sağlayıcısı
    /// </summary>
    public class HttpGetProvider : BaseProvider
    {
        /// <summary>
        /// Http get isteği gönderir
        /// </summary>
        /// <typeparam name="TResult">Http isteğinden dönecek yanıtın tipi</typeparam>
        /// <param name="url">Http isteği atılacak adres</param>
        /// <param name="cancellationToken">İsteğin iptal tokenı</param>
        /// <returns></returns>
        public virtual async Task<TResult> GetAsync<TResult>(string url, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Uri requestUri = GenerateUri(url);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json; charset=UTF-8";

            AppendHeaders(webRequest);

            HttpWebResponse webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();

            using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                string response = await streamReader.ReadToEndAsync();

                return JsonConvert.DeserializeObject<TResult>(response);
            }
        }
    }
}
