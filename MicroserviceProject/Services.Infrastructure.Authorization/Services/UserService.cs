using Infrastructure.Cryptography.Ciphers;
using Infrastructure.Security.Model;
using Services.Infrastructure.Authorization.Persistence.Sql.Exceptions;
using Services.Infrastructure.Authorization.Persistence.Sql.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Infrastructure.Authorization.Business.Services
{
    public class UserService : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly SessionRepository _sessionRepository;
        private readonly UserRepository _userRepository;

        public UserService(
            SessionRepository sessionRepository,
            UserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Kullanıcıyı asenkron olarak getirir
        /// </summary>
        /// <param name="token">Kullanıcının token değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<AuthenticatedUser> GetUserAsync(string token, CancellationTokenSource cancellationTokenSource)
        {
            AuthenticationSession session =
                await
                _sessionRepository
                .GetValidSessionAsync(token, cancellationTokenSource);

            if (session != null)
            {
                AuthenticatedUser user = await _userRepository.GetUserAsync(session.UserId, cancellationTokenSource);

                if (user != null)
                {
                    return new AuthenticatedUser()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        IsAdmin = user.IsAdmin,
                        Region = user.Region,
                        Token = new AuthenticationToken()
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
        /// Kullanıcının varlığını e-posta adresine göre kontrol eder
        /// </summary>
        /// <param name="email">E-posta adresi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<bool> CheckUserAsync(string email, CancellationTokenSource cancellationTokenSource)
        {
            return await _userRepository.CheckUserAsync(email, cancellationTokenSource);
        }

        /// <summary>
        /// Kullanıcı oluşturur ve ardından token verir
        /// </summary>
        /// <param name="credential">Kullanıcının kimlik bilgileri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task RegisterUserAsync(AuthenticationCredential credential, CancellationTokenSource cancellationTokenSource)
        {
            string passwordHash = SHA256Cryptography.Crypt(credential.Password);

            await _userRepository.RegisterAsync(credential.Email, passwordHash, cancellationTokenSource);
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
