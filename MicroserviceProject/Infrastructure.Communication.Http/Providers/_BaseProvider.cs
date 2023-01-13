
using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Http.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

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
            if (Queries != null && Queries.Any())
            {
                QueryBuilder queryBuilder = new QueryBuilder();

                foreach (var query in Queries)
                {
                    queryBuilder.Add(query.Key, query.Value);
                }

                url += queryBuilder.ToQueryString().Value;
            }

            return new Uri(url);
        }

        /// <summary>
        /// Http isteğine headerları ekler
        /// </summary>
        /// <param name="httpClient">Headerlar eklenecek web isteği</param>
        protected void AppendHeaders(HttpClient httpClient)
        {
            if (Headers != null && Headers.Any())
            {
                httpClient.DefaultRequestHeaders.Clear();

                foreach (HttpHeader header in Headers)
                {
                    if (!string.IsNullOrWhiteSpace(header.Value))
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
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
