using MicroserviceProject.Infrastructure.Cryptography.SHA256;
using MicroserviceProject.Model.Security;
using MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Exceptions;
using MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Providers
{
    /// <summary>
    /// MS SQL veritabanı sağlayıcısı
    /// </summary>
    public class SqlDataProvider
    {
        /// <summary>
        /// Oturum repository
        /// </summary>
        private readonly SessionRepository sessionRepository;

        /// <summary>
        /// Kullanıcı repository
        /// </summary>
        private readonly UserRepository userRepository;

        /// <summary>
        /// MS SQL veritabanı sağlayıcısı
        /// </summary>
        /// <param name="sessionRepository">Oturum repository</param>
        /// <param name="userRepository">Kullanıcı repository</param>
        public SqlDataProvider(
            SessionRepository sessionRepository,
            UserRepository userRepository)
        {
            this.sessionRepository = sessionRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Kullanıcıyı asenkron olarak getirir
        /// </summary>
        /// <param name="token">Kullanıcının token değeri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string token, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Session session =
                await
                sessionRepository
                .GetValidSessionAsync(token, cancellationToken);

            if (session != null)
            {
                User user = await userRepository.GetUserAsync(session.UserId, cancellationToken);

                if (user != null)
                {
                    return new User()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        IsAdmin = user.IsAdmin,
                        Region = user.Region,
                        Token = new Token()
                        {
                            TokenKey = session.Token,
                            ValidTo = session.ValidTo
                        },
                        SessionId = session.Id
                    };
                }
                else
                {
                    throw new UserNotFoundException();
                }
            }
            else
            {
                throw new SessionNotFoundException();
            }
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
                userRepository.GetUserAsync(credential.Email, passwordHash, cancellationToken);

            if (user != null)
            {
                //TODO: Önceki oturumlar silinecek.

                string token = Guid.NewGuid().ToString();

                DateTime validTo = DateTime.Now.AddMinutes(60);

                await
                sessionRepository
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
        /// Kullanıcı oluşturur ve ardından token verir
        /// </summary>
        /// <param name="credential">Kullanıcının kimlik bilgileri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<Token> RegisterUserAsync(Credential credential, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string passwordHash = SHA256Cryptography.Crypt(credential.Password);

            await userRepository.RegisterAsync(credential.Email, passwordHash, cancellationToken);

            return await GetTokenAsync(credential, cancellationToken);
        }

        /// <summary>
        /// Kullanıcının varlığını e-posta adresine göre kontrol eder
        /// </summary>
        /// <param name="email">E-posta adresi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<bool> CheckUserAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await userRepository.CheckUserAsync(email, cancellationToken);
        }
    }
}
