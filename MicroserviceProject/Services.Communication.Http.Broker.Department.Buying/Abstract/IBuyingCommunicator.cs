using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Buying.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Department.Buying.Abstract
{
    public interface IBuyingCommunicator : IDisposable
    {
        Task<ServiceResultModel<List<InventoryRequestModel>>> GetInventoryRequestsAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> ValidateCostInventoryAsync(
            ValidateCostInventoryCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateInventoryRequestAsync(
            CreateInventoryRequestCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource);
    }
}
