using Infrastructure.Cryptography.Ciphers;
using Infrastructure.Security.Authentication.JWT.Providers;
using Infrastructure.Security.Model;
using Services.Gateway.Public.Exceptions;
using Services.Gateway.Public.Repositories;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Gateway.Public.Services
{
    public class UserService
    {
        private readonly UserRepository userRepository;
        private readonly TokenProvider tokenProvider;

        public UserService(
            UserRepository userRepository,
            TokenProvider tokenProvider)
        {
            this.userRepository = userRepository;
            this.tokenProvider = tokenProvider;
        }

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
