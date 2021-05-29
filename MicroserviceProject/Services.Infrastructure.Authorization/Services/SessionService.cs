using Infrastructure.Cryptography.Ciphers;
using Infrastructure.Localization.Providers;
using Infrastructure.Security.Model;
using Infrastructure.Communication.Http.Wrapper;

using Services.Infrastructure.Authorization.Persistence.Sql.Exceptions;
using Services.Infrastructure.Authorization.Persistence.Sql.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Infrastructure.Authorization.Business.Services
{
    public class SessionService : BaseService, IDisposable, IDisposableInjections
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        private readonly SessionRepository _sessionRepository;
        private readonly UserRepository _userRepository;
        
        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

        public override string ServiceName { get; }
        public override string ApiServiceName { get; }

        public SessionService(
            TranslationProvider translationProvider,
            SessionRepository sessionRepository,
            UserRepository userRepository)
        {
            _translationProvider = translationProvider;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Kullanıcı kimliğine göre token verir
        /// </summary>
        /// <param name="credential">Kullanıcının kimlik bilgleri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<Token> GetTokenAsync(Credential credential, CancellationTokenSource cancellationTokenSource)
        {
            string passwordHash = SHA256Cryptography.Crypt(credential.Password);

            User user =
                await
                _userRepository.GetUserAsync(credential.Email, passwordHash, cancellationTokenSource);

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
                        cancellationTokenSource);

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

        public void DisposeInjections()
        {
            _sessionRepository.Dispose();
            _translationProvider.Dispose();
            _translationProvider.Dispose();
        }
    }
}
