using Infrastructure.Communication.Http.Endpoint.Abstract;

using Microsoft.AspNetCore.Http.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Infrastructure.Communication.Http.Broker
{
    public class BaseCaller
    {
        protected string GenerateQueryString(string baseUrl, Dictionary<string,string> httpQueries)
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            Dictionary<string, bool> queryChecks = httpQueries.ToDictionary(x => x.Key, y => false);

            foreach (KeyValuePair<string, string> query in httpQueries)
            {
                if (queryChecks.Any(x => x.Key == query.Key))
                    queryChecks[query.Key] = true;

                queryBuilder.Add(query.Key, query.Value);
            }

            if (queryChecks.Any(x => !x.Value))
            {
                throw new Exception();
            }

            string url = baseUrl + queryBuilder.ToQueryString();

            return url;
        }

        protected void GenerateHeaders(HttpClient httpClient, Dictionary<string,string> headers)
        {
            Dictionary<string, bool> headerChecks = headers.ToDictionary(x => x.Key, y => false);

            foreach (KeyValuePair<string, string> header in headers)
            {
                if (headerChecks.Any(x => x.Key == header.Key))
                    headerChecks[header.Key] = true;

                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            if (headerChecks.Any(x => !x.Value))
            {
                throw new Exception();
            }
        }
    }
}
