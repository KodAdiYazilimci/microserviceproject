using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Http.Extensions;

using System.Collections.Generic;
using System.Net.Http;

namespace Infrastructure.Communication.Http.Helpers
{
    public class HttpHelper
    {
        public static string GenerateQueryString(string baseUrl, List<HttpQueryModel> httpQueries)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            foreach (HttpQueryModel query in httpQueries)
            {
                if (!string.IsNullOrWhiteSpace(query.Value))
                    queryBuilder.Add(query.Name, query.Value);
            }

            string url = baseUrl + queryBuilder.ToQueryString();

            return url;
        }

        public static void GenerateHeaders(HttpClient httpClient, List<HttpHeaderModel> headers)
        {
            foreach (HttpHeaderModel header in headers)
            {
                httpClient.DefaultRequestHeaders.Remove(header.Name);
                httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
            }
        }
    }
}
