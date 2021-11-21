using Infrastructure.Security.Model;

using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Security.Authentication.Cookie.Abstract
{
    /// <summary>
    /// Kimlik verisi sağlayıcı arayüzü
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        /// Kullanıcı bilgisine göre oturum bilgilerini getirir
        /// </summary>
        /// <param name="credential">Kullanıcı bilgisi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        Task<AuthenticatedUser> GetUserAsync(AuthenticationCredential credential, CancellationTokenSource cancellationTokenSource);

        /// <summary>
        /// Token bilgisine göre oturumda bulunan kullanıcıyı verir
        /// </summary>
        Task<AuthenticatedUser> GetUserAsync(string token, CancellationTokenSource cancellationTokenSource);

        /// <summary>
        /// Token bazlı kullanıcı oturumunu önbellekte saklar
        /// </summary>
        /// <param name="userModel">Kullanıcının model nesnesi</param>
        void SetToCache(AuthenticatedUser userModel);

        /// <summary>
        /// Önbellekten bir kullanıcıyı siler
        /// </summary>
        /// <param name="token">Silinecek kullanıcının tokenı</param>
        void RemoveUserFromCache(string token);

        /// <summary>
        /// Bir kullanıcının geçerli token bazlı oturum bilgisini önbellekten çağırır
        /// </summary>
        /// <param name="token">Kullanıcının oturum anahtarı</param>
        /// <returns></returns>
        AuthenticatedUser GetUserFromCache(string token);
    }
}
