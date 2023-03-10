using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Production.Models;

namespace Services.Communication.Http.Broker.Department.Production.Abstract
{
    public interface IProductionCommunicator : IDisposable
    {
        Task<ServiceResultModel<List<ProductModel>>> GetProductsAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> ProduceProductAsync(
            ProduceProductCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateProductAsync(
            CreateProductCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> ReEvaluateProduceProductAsync(
            int referenceNumber,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);
    }
}
