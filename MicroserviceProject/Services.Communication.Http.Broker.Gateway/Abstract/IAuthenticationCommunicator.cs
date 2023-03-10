using Services.Communication.Http.Broker.Abstract;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Gateway.Abstract
{
    public interface IAuthenticationCommunicator
    {
        Task<string> GetServiceToken(CancellationTokenSource cancellationTokenSource);
    }
}
