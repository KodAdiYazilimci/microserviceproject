
using Infrastructure.Communication.Http.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Infrastructure.Communication.Http.Providers
{
    /// <summary>
    /// Http istekleri için ortak sağlayıcı
    /// </summary>
    public abstract class BaseProvider : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Http isteği esnasında kullanılacak headerlar
        /// </summary>
        public List<HttpHeader> Headers { get; set; }

        /// <summary>
        /// Http isteği esnasında kullanılacak QueryString parametreleri
        /// </summary>
        public List<HttpQuery> Queries { get; set; }

        /// <summary>
        /// Http isteğinde kullanulacak Uri oluşturur
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected Uri GenerateUri(string url)
        {
            Uri requestUri = new Uri(url);

            if (Queries != null && Queries.Any())
            {
                foreach (var query in Queries)
                {
                    if (string.IsNullOrEmpty(requestUri.Query))
                    {
                        url += "?";
                    }
                    else
                    {
                        url += "&";
                    }

                    url += query.Key;
                    url += "=";
                    url += query.Value;
                    requestUri = new Uri(url);
                }
            }

            return requestUri;
        }

        /// <summary>
        /// Http isteğine headerları ekler
        /// </summary>
        /// <param name="webRequest">Headerlar eklenecek web isteği</param>
        protected void AppendHeaders(HttpWebRequest webRequest)
        {
            if (Headers != null && Headers.Any())
            {
                foreach (HttpHeader header in Headers)
                {
                    webRequest.Headers.Add(header.Key, header.Value);
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
        public virtual void Dispose(bool disposing)
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
