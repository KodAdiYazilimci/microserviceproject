using Infrastructure.Persistence.InMemory.ServiceRoutes.Configuration;

using MicroserviceProject.Model.Communication.Moderator;

using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence.InMemory.ServiceRoutes.Persistence
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
                context.CallModels.Add(new CallModel()
                {
                    Id = 1,
                    CallType = "GET",
                    Endpoint = "http://localhost:15269/GetData",
                    QueryKeys = new List<QueryKey>() { new QueryKey() { Id = 1, CallModelId = 1, Key = "number" } },
                    ServiceName = "sampledataprovider.getdata"
                });

                context.CallModels.Add(new CallModel()
                {
                    Id = 2,
                    CallType = "POST",
                    Endpoint = "http://localhost:15269/PostData",
                    QueryKeys = new List<QueryKey>(),
                    ServiceName = "sampledataprovider.postdata"
                });

                context.SaveChanges();
            }
        }
    }
}
