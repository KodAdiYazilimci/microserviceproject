using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Persistence.Repositories.Sql;

using Microsoft.AspNetCore.Http.Extensions;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Endpoint.Abstract
{
    public interface IEndpoint
    {
        string Url { get; set; }
        string Name { get; set; }
        List<HttpHeader> Headers { get; set; }
        List<HttpQuery> Queries { get; set; }
        IRequiredAuthentication RequiredAuthentication { get; set; }
    }

    public interface IRequiredAuthentication
    {
        void SetAuthentication(HttpClient httpClient);
    }

    class TestEndpoint : IEndpoint
    {
        public TestEndpoint(IRequiredAuthentication requiredAuthentication)
        {
            RequiredAuthentication = requiredAuthentication;
        }

        public string Url { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<HttpHeader> Headers { get; set; } = new List<HttpHeader>();
        public List<HttpQuery> Queries { get; set; } = new List<HttpQuery>();
        public IRequiredAuthentication? RequiredAuthentication { get; set; } = null;
    }

    class TokenAuthentication : IRequiredAuthentication
    {
        string token = string.Empty;
        public TokenAuthentication(string token)
        {
            this.token = token;
        }

        public void SetAuthentication(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
        }
    }

    public interface IServiceCaller<TResult>
    {
        TResult Call(IEndpoint endpoint);
        TResult Call(IEndpoint endpoint, List<HttpHeader> headers);
        TResult Call(IEndpoint endpoint, List<HttpQuery> httpQueries);
        public TResult Call(IEndpoint endpoint, List<HttpHeader> headers, List<HttpQuery> httpQueries);
    }

    class HttpGetCaller<TResult> : IServiceCaller<TResult>
    {
        HttpClient httpClient = new HttpClient();

        public TResult Call(IEndpoint endpoint)
        {
            if (endpoint.Headers.Any() || endpoint.Queries.Any())
            {
                throw new Exception();
            }

            endpoint.RequiredAuthentication.SetAuthentication(httpClient);

            Task<HttpResponseMessage> getTask = httpClient.GetAsync(endpoint.Url);

            getTask.Wait();

            return JsonConvert.DeserializeObject<TResult>(getTask.Result.Content.ReadAsStringAsync().Result);
        }

        public TResult Call(IEndpoint endpoint, List<HttpHeader> headers)
        {
            if (endpoint.Queries.Any())
            {
                throw new Exception();
            }

            GenerateHeaders(headers);

            return Call(endpoint);
        }

        private void GenerateHeaders(List<HttpHeader> headers)
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

        public TResult Call(IEndpoint endpoint, List<HttpQuery> httpQueries)
        {
            if (endpoint.Headers.Any())
            {
                throw new Exception();
            }

            GenerateQueryString(endpoint, httpQueries);

            return Call(endpoint);
        }

        public TResult Call(IEndpoint endpoint, List<HttpHeader> headers, List<HttpQuery> httpQueries)
        {
            headers.ForEach(header =>
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            });

            GenerateQueryString(endpoint, httpQueries);

            return Call(endpoint);
        }

        private void GenerateQueryString(IEndpoint endpoint, List<HttpQuery> httpQueries)
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
    }

    class RouteProvider
    {
        private readonly ServiceRouteRepository serviceRouteRepository;
        private List<IEndpoint> endpoints = new List<IEndpoint>();

        public RouteProvider(ServiceRouteRepository serviceRouteRepository)
        {
            this.serviceRouteRepository = serviceRouteRepository;
        }

        public async Task<IEndpoint?> GetEndpoint(Type endpointType, CancellationTokenSource cancellationTokenSource)
        {
            if (endpointType is IEndpoint)
            {
                string endpoint = endpointType.Name;

                if (!endpoints.Any(x => x.Name == endpoint))
                {
                    var route = await serviceRouteRepository.GetServiceRouteAsync(endpoint, cancellationTokenSource);

                    if (route != null)
                    {
                        IEndpoint foundEnpoint = (IEndpoint)Activator.CreateInstance(endpointType);

                        if (foundEnpoint != null)
                        {
                            foundEnpoint.Name = endpoint;
                            foundEnpoint.Url = route.Endpoint;
                            foundEnpoint.Queries = route.QueryKeys.Select(x => new HttpQuery()
                            {
                                Key = x.Key
                            }).ToList();
                            foundEnpoint.Headers = new List<HttpHeader>();
                            foundEnpoint.RequiredAuthentication = new TokenAuthentication(string.Empty);

                            endpoints.Add(foundEnpoint);

                            return foundEnpoint;
                        }
                    }
                }
                else
                    return endpoints.Where(x => x.Name == endpoint).FirstOrDefault();
            }

            return null;
        }
    }

    class Test
    {
        async Task TestM()
        {
            IRequiredAuthentication requiredAuthentication = new TokenAuthentication("123");
            IEndpoint? testEndpoint = new TestEndpoint(requiredAuthentication);

            IServiceCaller<string> getCaller = new HttpGetCaller<string>();
            string result = getCaller.Call(testEndpoint);
            result = getCaller.Call(testEndpoint, new List<HttpHeader>() { });
            result = getCaller.Call(testEndpoint, new List<HttpHeader>(), new List<HttpQuery>());

            //

            RouteProvider routeProvider = new RouteProvider(new ServiceRouteRepository(null));
            testEndpoint = await routeProvider.GetEndpoint(typeof(TestEndpoint), new CancellationTokenSource());

            if (testEndpoint != null)
                result = getCaller.Call(testEndpoint);
        }
    }
}
