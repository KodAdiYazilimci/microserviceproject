using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Gateway.Public.Models;

namespace Services.Communication.Http.Broker.Gateway.Public.Abstract
{
    public interface IPublicGatewayCommunicator
    {
        Task<ServiceResultModel<List<DepartmentModel>>> GetDepartmentsAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);
    }
}
