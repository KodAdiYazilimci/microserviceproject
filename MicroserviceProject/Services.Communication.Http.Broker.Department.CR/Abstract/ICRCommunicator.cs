using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.CR.Models;

namespace Services.Communication.Http.Broker.Department.CR.Abstract
{
    public interface ICRCommunicator
    {
        Task<ServiceResultModel<List<CustomerModel>>> GetCustomersAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateCustomerAsync(
            CreateCustomerCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource);
    }
}
