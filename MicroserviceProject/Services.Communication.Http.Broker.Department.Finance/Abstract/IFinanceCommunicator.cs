using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Finance.Models;

namespace Services.Communication.Http.Broker.Department.Finance.Abstract
{
    public interface IFinanceCommunicator : IDisposable
    {
        Task<ServiceResultModel> CreateCostAsync(
            CreateCostCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateProductionRequestAsync(
           CreateProductionRequestCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel<List<DecidedCostModel>>> GetDecidedCostsAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> DecideCostAsync(
            DecideCostCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource);
    }
}
