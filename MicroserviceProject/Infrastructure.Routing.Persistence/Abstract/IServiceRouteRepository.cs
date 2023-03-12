using Infrastructure.Routing.Models;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Routing.Persistence.Abstract
{
    public interface IServiceRouteRepository : IDisposable
    {
        Task<List<ServiceRouteModel>> GetServiceRoutesAsync(CancellationTokenSource cancellationTokenSource);
        Task<ServiceRouteModel> GetServiceRouteAsync(string serviceName, CancellationTokenSource cancellationTokenSource);
    }
}
