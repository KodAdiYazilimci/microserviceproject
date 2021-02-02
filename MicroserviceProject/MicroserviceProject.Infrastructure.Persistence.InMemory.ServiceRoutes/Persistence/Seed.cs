using MicroserviceProject.Infrastructure.Persistence.InMemory.ServiceRoutes.Configuration;

using MicroserviceProject.Model.Communication.Moderator;

using System.Collections.Generic;
using System.Linq;

namespace MicroserviceProject.Infrastructure.Persistence.InMemory.ServiceRoutes.Persistence
{
    /// <summary>
    /// InMemory veritabanına temel verileri yükleyen sınıf
    /// </summary>
    public class Seed
    {
        /// <summary>
        /// Temel verileri yükler
        /// </summary>
        /// <param name="context"></param>
        public void InsertInitialData(ServiceRouteContext context)
        {
            if (!context.CallModels.Any())
            {
                context.CallModels.Add(new ServiceRoute()
                {
                    Id = 1,
                    CallType = "GET",
                    Endpoint = "http://localhost:15269/GetData",
                    QueryKeys = new List<RouteQuery>() { new RouteQuery() { Id = 1, CallModelId = 1, Key = "number" } },
                    ServiceName = "sampledataprovider.getdata"
                });

                context.CallModels.Add(new ServiceRoute()
                {
                    Id = 2,
                    CallType = "POST",
                    Endpoint = "http://localhost:15269/PostData",
                    QueryKeys = new List<RouteQuery>(),
                    ServiceName = "sampledataprovider.postdata"
                });

                context.CallModels.Add(new ServiceRoute()
                {
                    Id = 3,
                    CallType = "GET",
                    Endpoint = "http://localhost:16859/Auth/GetUser",
                    QueryKeys = new List<RouteQuery>() { new RouteQuery() { Id = 2, CallModelId = 3, Key = "token" } },
                    ServiceName = "authorization.getuser"
                });

                context.CallModels.Add(new ServiceRoute()
                {
                    Id = 4,
                    CallType = "POST",
                    QueryKeys = new List<RouteQuery>(),
                    Endpoint = "http://localhost:16859/Auth/GetToken",
                    ServiceName = "authorization.gettoken"
                });

                context.SaveChanges();
            }
        }
    }
}
