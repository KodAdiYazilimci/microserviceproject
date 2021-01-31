using MicroserviceProject.Infrastructure.Cryptography.SHA256;
using MicroserviceProject.Model.Security;
using MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Exceptions;
using MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Security.Authorization.Business.Services
{
    public class SessionService
    {
        private readonly SessionRepository _sessionRepository;
        private readonly UserRepository _userRepository;

        public SessionService(
            SessionRepository sessionRepository,
            UserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Kullanıcı kimliğine göre token verir
        /// </summary>
        /// <param name="credential">Kullanıcının kimlik bilgleri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<Token> GetTokenAsync(Credential credential, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string passwordHash = SHA256Cryptography.Crypt(credential.Password);

            User user =
                await
                _userRepository.GetUserAsync(credential.Email, passwordHash, cancellationToken);

            if (user != null)
            {
                //TODO: Önceki oturumlar silinecek.

                string token = Guid.NewGuid().ToString();

                DateTime validTo = DateTime.Now.AddMinutes(60);

                await
                _sessionRepository
                    .InsertSessionAsync(
                        user.Id,
                        token,
                        validTo,
                        credential.IpAddress,
                        credential.UserAgent,
                        cancellationToken);

                return new Token()
                {
                    TokenKey = token,
                    ValidTo = validTo
                };
            }
            else
            {
                throw new UserNotFoundException("Kullanıcı adı veya şifre yanlış!");
            }
        }
    }
}
