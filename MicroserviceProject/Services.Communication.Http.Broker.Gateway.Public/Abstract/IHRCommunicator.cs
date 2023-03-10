using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Gateway.Public.Models;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Gateway.Gateway.Public.Abstract
{
    public interface IHRCommunicator : IDisposable
    {
        Task<ServiceResultModel<List<DepartmentModel>>> GetDepartmentsAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(string tokenKey, CancellationTokenSource cancellationTokenSource);
    }
}
