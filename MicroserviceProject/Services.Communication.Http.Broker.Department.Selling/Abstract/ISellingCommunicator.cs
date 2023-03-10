using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Selling.Models;

namespace Services.Communication.Http.Broker.Department.Selling.Abstract
{
    public interface ISellingCommunicator : IDisposable
    {
        Task<ServiceResultModel<List<SellModel>>> GetSoldsAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateSellingAsync(
            CreateSellingCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> NotifyProductionRequestAsync(
            NotifyProductionRequestCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource);
    }
}
