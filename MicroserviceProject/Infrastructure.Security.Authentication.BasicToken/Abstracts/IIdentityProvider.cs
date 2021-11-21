using Infrastructure.Security.Model;

using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Security.Authentication.BasicToken.Abstracts
{
    /// <summary>
    /// Kimlik verisi sağlayıcı arayüzü
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        /// Kimlik denetiminin başarısız olması durumunda gösterilecek mesaj
        /// </summary>
        string AuthenticationFailMessage { get; set; }

        /// <summary>
        /// Oturumda bulunan kullanıcı
        /// </summary>
        Task<AuthenticatedUser> GetUserAsync(CancellationTokenSource cancellationTokenSource);
    }
}
