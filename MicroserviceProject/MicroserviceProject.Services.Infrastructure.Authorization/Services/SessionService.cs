using MicroserviceProject.Infrastructure.Cryptography.Ciphers;
using MicroserviceProject.Infrastructure.Security.Model;
using MicroserviceProject.Services.Infrastructure.Authorization.Persistence.Sql.Exceptions;
using MicroserviceProject.Services.Infrastructure.Authorization.Persistence.Sql.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Infrastructure.Authorization.Business.Services
{
    public class SessionService : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    _sessionRepository.Dispose();
                    _userRepository.Dispose();
                }

                disposed = true;
            }
        }
    }
}
