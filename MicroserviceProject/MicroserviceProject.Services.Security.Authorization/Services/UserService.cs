﻿using MicroserviceProject.Infrastructure.Cryptography.SHA256;
using MicroserviceProject.Infrastructure.Security.Model;
using MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Exceptions;
using MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Repositories;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Security.Authorization.Business.Services
{
    public class UserService
    {
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
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string token, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Session session =
                await
                _sessionRepository
                .GetValidSessionAsync(token, cancellationToken);

            if (session != null)
            {
                User user = await _userRepository.GetUserAsync(session.UserId, cancellationToken);

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
        /// Kullanıcının varlığını e-posta adresine göre kontrol eder
        /// </summary>
        /// <param name="email">E-posta adresi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<bool> CheckUserAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _userRepository.CheckUserAsync(email, cancellationToken);
        }

        /// <summary>
        /// Kullanıcı oluşturur ve ardından token verir
        /// </summary>
        /// <param name="credential">Kullanıcının kimlik bilgileri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task RegisterUserAsync(Credential credential, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string passwordHash = SHA256Cryptography.Crypt(credential.Password);

            await _userRepository.RegisterAsync(credential.Email, passwordHash, cancellationToken);
        }
    }
}