using MicroserviceProject.Infrastructure.Security.Authentication.JWT.Providers;
using MicroserviceProject.Infrastructure.Security.Model;
using MicroserviceProject.Services.Gateway.Public.Exceptions;
using MicroserviceProject.Services.Gateway.Public.Repositories;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Gateway.Public.Services
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
            User user = await userRepository.GetUserAsync(username, password, cancellationTokenSource);

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
