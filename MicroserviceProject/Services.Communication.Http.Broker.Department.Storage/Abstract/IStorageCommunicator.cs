using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Storage.Models;

namespace Services.Communication.Http.Broker.Department.Storage.Abstract
{
    public interface IStorageCommunicator
    {
        Task<ServiceResultModel<StockModel>> GetStockAsync(
            int productId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> DescendStockAsync(
           DescendProductStockCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateStockAsync(
           CreateStockCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource);
    }
}
