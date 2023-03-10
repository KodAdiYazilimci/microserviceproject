using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Department.IT.Abstract
{
    public interface IITCommunicator : IDisposable
    {
        Task<ServiceResultModel<List<ITInventoryModel>>> GetInventoriesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateInventoryAsync(
            ITCreateInventoryCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel<List<ITDefaultInventoryForNewWorkerModel>>> GetInventoriesForNewWorkerAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> AssignInventoryToWorkerAsync(
           ITAssignInventoryToWorkerCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateDefaultInventoryForNewWorkerAsync(
           ITCreateDefaultInventoryForNewWorkerCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> InformInventoryRequestAsync(
            ITInformInventoryRequestCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource);
    }
}
