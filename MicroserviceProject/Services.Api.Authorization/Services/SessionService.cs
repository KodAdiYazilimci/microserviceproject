
using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Communication.Http.Wrapper.Disposing;
using Infrastructure.Cryptography.Ciphers;
using Infrastructure.Localization.Translation.Provider;
using Infrastructure.Transaction.UnitOfWork.EntityFramework;

using Microsoft.EntityFrameworkCore;

using Services.Api.Infrastructure.Authorization.Configuration.Persistence;
using Services.Api.Infrastructure.Authorization.Constants;
using Services.Api.Infrastructure.Authorization.Entities.EntityFramework;
using Services.Api.Infrastructure.Authorization.Persistence.Sql.Exceptions;
using Services.Api.Infrastructure.Authorization.Repositories;
using Services.Communication.Http.Broker.Authorization.Models;
using Services.Communication.Mq.Rabbit.Queue.Authorization.Models;
using Services.Communication.Mq.Rabbit.Queue.Authorization.Publishers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Infrastructure.Authorization.Business.Services
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
        public override string ServiceName => "Services.Api.Infrastructure.Authorization.Business.Services.SessionService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Api.Infrastructure.Authorization";

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

        private readonly InformInvalidTokenPublisher _informInvalidTokenPublisher;

        public SessionService(
            SessionRepository sessionRepository,
            UserRepository userRepository,
            RedisCacheDataProvider redisCacheDataProvider,
            IUnitOfWork<AuthContext> unitOfWork,
            TranslationProvider translationProvider,
            InformInvalidTokenPublisher informInvalidTokenPublisher)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _redisCacheDataProvider = redisCacheDataProvider;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;
            _informInvalidTokenPublisher = informInvalidTokenPublisher;
        }

        /// <summary>
        /// Kullanıcı kimliğine göre token verir
        /// </summary>
        /// <param name="credential">Kullanıcının kimlik bilgleri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<TokenModel> GetTokenAsync(CredentialModel credential, CancellationTokenSource cancellationTokenSource)
        {
            if (string.IsNullOrEmpty(credential.GrantType) || credential.GrantType.ToLower() == GrantType.Password)
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
                    List<Session> oldSessions = await _unitOfWork.Context.Sessions.Where(x => x.DeleteDate == null && x.IsValid && x.UserId == user.Id).ToListAsync();

                    foreach (Session session in oldSessions)
                    {
                        session.IsValid = false;

                        _informInvalidTokenPublisher.AddToBuffer(new InvalidTokenQueueModel()
                        {
                            TokenKey = session.Token
                        });
                    }

                    Session newSession = new Session()
                    {
                        UserId = user.Id,
                        Token = Guid.NewGuid().ToString(),
                        ValidTo = DateTime.Now.AddMinutes(60),
                        IsValid = true,
                        IpAddress = credential.IpAddress,
                        UserAgent = credential.UserAgent,
                        Region = credential.Region,
                        GrantType = "password",
                        Scope = credential.Scope,
                        RefreshIndex = 0,
                        RefreshToken = Guid.NewGuid().ToString(),
                    };

                    _unitOfWork.Context.Sessions.Add(newSession);

                    await _unitOfWork.SaveAsync(cancellationTokenSource);

                    await _informInvalidTokenPublisher.PublishBufferAsync(cancellationTokenSource);

                    return new TokenModel()
                    {
                        TokenKey = newSession.Token,
                        ValidTo = newSession.ValidTo,
                        RefreshToken = newSession.RefreshToken,
                        Scope = newSession.Scope
                    };
                }
                else
                {
                    throw new UserNotFoundException("Kullanıcı adı veya şifre yanlış!");
                }
            }
            else if (credential.GrantType.ToLower() == GrantType.RefreshToken)
            {
                Session oldSession =
                    await _unitOfWork.Context.Sessions.FirstOrDefaultAsync(x => x.DeleteDate == null && x.RefreshToken == credential.RefreshToken && x.IsValid);

                if (oldSession != null)
                {
                    Session newSession = new Session();
                    newSession.BeforeSessionId = oldSession.Id;
                    newSession.GrantType = "refresh_token";
                    newSession.IpAddress = oldSession.IpAddress;
                    newSession.IsValid = true;
                    newSession.RefreshIndex = oldSession.RefreshIndex + 1;
                    newSession.RefreshToken = Guid.NewGuid().ToString();
                    newSession.Region = oldSession.Region;
                    newSession.Scope = oldSession.Scope;
                    newSession.Token = Guid.NewGuid().ToString();
                    newSession.UserAgent = oldSession.UserAgent;
                    newSession.UserId = oldSession.UserId;
                    newSession.ValidTo = DateTime.Now.AddMinutes(60);

                    _unitOfWork.Context.Sessions.Add(newSession);

                    oldSession.IsValid = false;

                    await _unitOfWork.SaveAsync(cancellationTokenSource);

                    return new TokenModel()
                    {
                        TokenKey = newSession.Token,
                        ValidTo = newSession.ValidTo,
                        RefreshToken = newSession.RefreshToken,
                        Scope = newSession.Scope
                    };
                }
                else
                {
                    throw new SessionNotFoundException();
                }
            }
            else
            {
                throw new UndefinedGrantTypeException();
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
