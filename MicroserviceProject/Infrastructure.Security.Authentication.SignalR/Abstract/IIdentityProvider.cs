
using Infrastructure.Security.Model;

using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Security.Authentication.SignalR.Abstract
{
    public interface IIdentityProvider
    {
        Task<AuthenticatedUser> GetUserAsync(string token, CancellationTokenSource cancellationTokenSource);

        /// <summary>
        /// Token bazlı kullanıcı oturumunu önbellekte saklar
        /// </summary>
        /// <param name="userModel">Kullanıcının model nesnesi</param>
        void SetToCache(AuthenticatedUser userModel);

        /// <summary>
        /// Bir kullanıcının geçerli token bazlı oturum bilgisini önbellekten çağırır
        /// </summary>
        /// <param name="token">Kullanıcının oturum anahtarı</param>
        /// <returns></returns>
        AuthenticatedUser GetUserFromCache(string token);
    }
}
