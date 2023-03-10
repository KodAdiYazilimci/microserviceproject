using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Gateway.Gateway.Public.Abstract;
using Services.Communication.Http.Broker.Gateway.Public.Abstract;
using Services.Communication.Http.Broker.Gateway.Public.Models;

namespace Services.Communication.Http.Broker.Gateway.Public
{
    public class PublicGatewayCommunicator : IPublicGatewayCommunicator
    {
        private readonly IHRCommunicator _hrCommunicator;

        public PublicGatewayCommunicator(IHRCommunicator hrCommunicator)
        {
            _hrCommunicator = hrCommunicator;
        }

        public Task<ServiceResultModel<List<DepartmentModel>>> GetDepartmentsAsync(string transactionIdentity, CancellationTokenSource cancellationTokenSource)
        {
            return _hrCommunicator.GetDepartmentsAsync(transactionIdentity, cancellationTokenSource);
        }

    }
}
