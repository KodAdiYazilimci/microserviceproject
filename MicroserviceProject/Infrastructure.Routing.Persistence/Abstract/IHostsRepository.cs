using Infrastructure.Routing.Models;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Routing.Persistence.Abstract
{
    public interface IHostsRepository : IDisposable
    {
        Task<List<HostModel>> GetServiceHostsAsync(CancellationTokenSource cancellationTokenSource);
    }
}
