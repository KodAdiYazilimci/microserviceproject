
using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Communication.Http.Wrapper.Disposing;
using Infrastructure.Cryptography.Ciphers;
using Infrastructure.Localization.Translation.Provider;
using Infrastructure.Transaction.UnitOfWork.EntityFramework;

using Microsoft.EntityFrameworkCore;

using Services.Api.Infrastructure.Authorization.Configuration.Persistence;
using Services.Api.Infrastructure.Authorization.Entities.EntityFramework;
using Services.Api.Infrastructure.Authorization.Persistence.Sql.Exceptions;
using Services.Api.Infrastructure.Authorization.Repositories;
using Services.Communication.Http.Broker.Authorization.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Infrastructure.Authorization.Business.Services
{
    public class UserService : BaseService, IAsyncDisposable, IDisposableInjectionsAsync
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Api.Infrastructure.Authorization.Business.Services.UserService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Api.Infrastructure.Authorization";

        /// <summary>
        /// Rediste tutulan önbellek yönetimini sağlayan sınıf
        /// </summary>
        private readonly IDistrubutedCacheProvider _redisCacheDataProvider;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IEfUnitOfWork<AuthContext> _unitOfWork;

        private readonly SessionRepository _sessionRepository;
        private readonly UserRepository _userRepository;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

        public UserService(
            SessionRepository sessionRepository,
            UserRepository userRepository,
            IDistrubutedCacheProvider redisCacheDataProvider,
            IEfUnitOfWork<AuthContext> unitOfWork,
            TranslationProvider translationProvider)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _redisCacheDataProvider = redisCacheDataProvider;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;
        }

        /// <summary>
        /// Kullanıcıyı asenkron olarak getirir
        /// </summary>
        /// <param name="token">Kullanıcının token değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<UserModel> GetUserAsync(string token, CancellationTokenSource cancellationTokenSource)
        {
            Session session =
                _unitOfWork.Context.Sessions.FirstOrDefault(x => x.DeleteDate == null && x.IsValid && x.Token == token && x.ValidTo > DateTime.UtcNow);

            if (session != null)
            {
                //User user = await _userRepository.GetUserAsync(session.UserId, cancellationTokenSource);

                UserModel user = await (from usr in _unitOfWork.Context.Users
                                        where usr.DeleteDate == null && usr.Id == session.UserId
                                        select new UserModel()
                                        {
                                            Id = usr.Id,
                                            Email = usr.Email,
                                            Region = session.Region,
                                            Claims = (from c in _unitOfWork.Context.Claims
                                                      join ct in _unitOfWork.Context.ClaimTypes
                                                      on c.ClaimTypeId equals ct.Id
                                                      where c.DeleteDate == null && ct.DeleteDate == null && c.UserId == usr.Id
                                                      select new ClaimModel()
                                                      {
                                                          Name = ct.Name,
                                                          Value = c.Value
                                                      }).ToList(),
                                            Roles = (from r in _unitOfWork.Context.Roles
                                                     join ur in _unitOfWork.Context.UserRoles
                                                      on r.Id equals ur.RoleId
                                                     where r.DeleteDate == null && ur.DeleteDate == null && ur.UserId == usr.Id
                                                     select new RoleModel()
                                                     {
                                                         Name = r.Name
                                                     }).ToList(),
                                            SessionId = session.Id,
                                            Token = new TokenModel()
                                            {
                                                TokenKey = session.Token,
                                                ValidTo = session.ValidTo
                                            }
                                        }).FirstOrDefaultAsync();

                if (user != null)
                {
                    user.Claims.Add(new ClaimModel() { Name = ClaimTypes.UserData, Value = user.Token.TokenKey });

                    return user;
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
            return await _unitOfWork.Context.Users.AnyAsync(x => x.DeleteDate == null && x.Email == email);
        }

        /// <summary>
        /// Kullanıcı oluşturur ve ardından token verir
        /// </summary>
        /// <param name="credential">Kullanıcının kimlik bilgileri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task RegisterUserAsync(CredentialModel credential, CancellationTokenSource cancellationTokenSource)
        {
            string passwordHash = SHA256Cryptography.Crypt(credential.Password);

            _unitOfWork.Context.Users.Add(new User()
            {
                Email = credential.Email,
                Password = passwordHash
            });

            await _unitOfWork.SaveAsync(cancellationTokenSource);
        }

        public async Task DisposeInjectionsAsync()
        {
            _redisCacheDataProvider.Dispose();
            await _sessionRepository.DisposeAsync();
            await _userRepository.DisposeAsync();
            await _unitOfWork.DisposeAsync();
            _translationProvider.Dispose();
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
    }
}
