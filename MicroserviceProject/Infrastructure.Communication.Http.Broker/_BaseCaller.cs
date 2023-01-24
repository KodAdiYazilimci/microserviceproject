using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Http.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Infrastructure.Communication.Http.Broker
{
    public class BaseCaller
    {
        protected void GenerateQueryString(HttpClient httpClient, IEndpoint endpoint, List<HttpQuery> httpQueries)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            Dictionary<string, bool> queryChecks = httpQueries.ToDictionary(x => x.Key, y => false);

            httpQueries.ForEach(query =>
            {
                if (queryChecks.Any(x => x.Key == query.Key))
                    queryChecks[query.Key] = true;

                queryBuilder.Add(query.Key, query.Value);
            });

            if (queryChecks.Any(x => !x.Value))
            {
                throw new Exception();
            }

            string url = endpoint.Url + queryBuilder.ToQueryString();

            httpClient.BaseAddress = new Uri(url);
        }

        protected void GenerateHeaders(HttpClient httpClient, List<HttpHeader> headers)
        {
            Dictionary<string, bool> headerChecks = headers.ToDictionary(x => x.Key, y => false);

            headers.ForEach(header =>
            {
                if (headerChecks.Any(x => x.Key == header.Key))
                    headerChecks[header.Key] = true;

                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            });

            if (headerChecks.Any(x => !x.Value))
            {
                throw new Exception();
            }
        }
    }
}
