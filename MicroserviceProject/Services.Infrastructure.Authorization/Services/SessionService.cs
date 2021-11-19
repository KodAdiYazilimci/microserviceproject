
using Communication.Http.Authorization.Models;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Communication.Http.Wrapper.Disposing;
using Infrastructure.Cryptography.Ciphers;
using Infrastructure.Localization.Providers;
using Infrastructure.Transaction.UnitOfWork.EntityFramework;

using Microsoft.EntityFrameworkCore;

using Services.Infrastructure.Authorization.Configuration.Persistence;
using Services.Infrastructure.Authorization.Entities.EntityFramework;
using Services.Infrastructure.Authorization.Persistence.Sql.Exceptions;
using Services.Infrastructure.Authorization.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Infrastructure.Authorization.Business.Services
{
    public class SessionService : BaseService, IDisposable, IDisposableInjectionsAsync
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Infrastructure.Authorization.Business.Services.SessionService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Infrastructure.Authorization";

        /// <summary>
        /// Rediste tutulan önbellek yönetimini sağlayan sınıf
        /// </summary>
        private readonly RedisCacheDataProvider _redisCacheDataProvider;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork<AuthContext> _unitOfWork;

        private readonly SessionRepository _sessionRepository;
        private readonly UserRepository _userRepository;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

        public SessionService(
            SessionRepository sessionRepository,
            UserRepository userRepository,
            RedisCacheDataProvider redisCacheDataProvider,
            IUnitOfWork<AuthContext> unitOfWork,
            TranslationProvider translationProvider)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _redisCacheDataProvider = redisCacheDataProvider;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;
        }

        /// <summary>
        /// Kullanıcı kimliğine göre token verir
        /// </summary>
        /// <param name="credential">Kullanıcının kimlik bilgleri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<TokenModel> GetTokenAsync(CredentialModel credential, CancellationTokenSource cancellationTokenSource)
        {
            string passwordHash = SHA256Cryptography.Crypt(credential.Password);

            User user =
                await
                _unitOfWork
                .Context
                .Users
                .FirstOrDefaultAsync(x =>
                                        x.DeleteDate == null
                                        &&
                                        x.Email == credential.Email
                                        &&
                                        x.Password == passwordHash, cancellationTokenSource.Token);

            if (user != null)
            {
                //TODO: Önceki oturumlar silinecek.

                string token = Guid.NewGuid().ToString();

                DateTime validTo = DateTime.Now.AddMinutes(60);

                _unitOfWork.Context.Sessions.Add(new Session()
                {
                    UserId = user.Id,
                    Token = token,
                    ValidTo = validTo,
                    IsValid = true,
                    IpAddress = credential.IpAddress,
                    UserAgent = credential.UserAgent,
                    Region = credential.Region
                });

                await _unitOfWork.SaveAsync(cancellationTokenSource);

                return new TokenModel()
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
        /// <returns></returns>
        public ValueTask DisposeAsync()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;
                }
            }
        }


        public async Task DisposeInjectionsAsync()
        {
            _redisCacheDataProvider.Dispose();
            _translationProvider.Dispose();
            await _userRepository.DisposeAsync();
            await _sessionRepository.DisposeAsync();
            await _unitOfWork.DisposeAsync();
        }
    }
}
