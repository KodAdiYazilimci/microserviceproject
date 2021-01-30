using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Communication.Http.Providers
{
    /// <summary>
    /// Http post isteği sağlayıcısı
    /// </summary>
    public class HttpPostProvider : BaseProvider
    {
        /// <summary>
        /// Http post isteği gönderir
        /// </summary>
        /// <typeparam name="TResult">Http isteğinden dönecek yanıtın tipi</typeparam>
        /// <typeparam name="TPostData">Http isteği içerisinde gönderilecek datanın tipi</typeparam>
        /// <param name="url">Http isteği atılacak adres</param>
        /// <param name="postData">Http isteği içerisinde gönderilecek data</param>
        /// <param name="cancellationToken">İsteğin iptal tokenı</param>
        /// <returns></returns>
        public virtual async Task<TResult> PostAsync<TResult, TPostData>(string url, TPostData postData, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Uri requestUri = GenerateUri(url);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json; charset=UTF-8";

            AppendHeaders(webRequest);

            using (StreamWriter streamWriter = new StreamWriter(await webRequest.GetRequestStreamAsync()))
            {
                string jsonBody = JsonConvert.SerializeObject(postData);

                await streamWriter.WriteAsync(jsonBody);
            }

            HttpWebResponse webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();

            using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                string response = await streamReader.ReadToEndAsync();

                return JsonConvert.DeserializeObject<TResult>(response);
            }
        }
    }
}
