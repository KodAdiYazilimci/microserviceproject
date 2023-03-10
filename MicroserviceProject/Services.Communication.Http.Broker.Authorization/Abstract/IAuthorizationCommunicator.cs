using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Authorization.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Authorization.Abstract
{
    public interface IAuthorizationCommunicator
    {
        Task<ServiceResultModel<TokenModel>> GetTokenAsync(
            CredentialModel credential,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel<UserModel>> GetUserAsync(
            string headerToken,
            CancellationTokenSource cancellationTokenSource);
    }
}
