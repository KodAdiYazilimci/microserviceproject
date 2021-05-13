using Infrastructure.Cryptography.Ciphers;
using Infrastructure.Security.Authentication.JWT.Providers;
using Infrastructure.Security.Model;
using Services.Gateway.Public.Exceptions;
using Services.Gateway.Public.Repositories;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Gateway.Public.Services
{
    /// <summary>
    /// Kullanıcı servisi
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// Kullanıcı repository sınıfı nesnesi
        /// </summary>
        private readonly UserRepository userRepository;

        /// <summary>
        /// Token sağlayıcı sınıfı nesnesi
        /// </summary>
        private readonly TokenProvider tokenProvider;

        /// <summary>
        /// Kullanıcı servisi
        /// </summary>
        /// <param name="userRepository">Kullanıcı repository sınıfı nesnesi</param>
        /// <param name="tokenProvider">Token sağlayıcı sınıfı nesnesi</param>
        public UserService(
            UserRepository userRepository,
            TokenProvider tokenProvider)
        {
            this.userRepository = userRepository;
            this.tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Kullanıcı için token verir
        /// </summary>
        /// <param name="username">Kullanıcının adı</param>
        /// <param name="password">Kullanıcının parolası</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<Token> GetTokenAsync(string username, string password, CancellationTokenSource cancellationTokenSource)
        {
            string passwordHash = SHA256Cryptography.Crypt(password);

            User user = await userRepository.GetUserAsync(username, passwordHash, cancellationTokenSource);

            if (user != null)
            {
                return tokenProvider.GetSecurityToken(user);
            }
            else
            {
                throw new UserNotFoundException();
            }
        }
    }
}
