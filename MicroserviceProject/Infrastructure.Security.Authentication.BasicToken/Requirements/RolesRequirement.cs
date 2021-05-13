
using Infrastructure.Security.Authentication.BasicToken.Abstracts;

using Microsoft.AspNetCore.Authorization;

using System;

namespace Infrastructure.Security.Authentication.BasicToken.Requirements
{
    /// <summary>
    /// Kimliğe ait rol gereksinimini tanımlayan sınıf
    /// </summary>
    public class RolesRequirement : IAuthorizationRequirement, IIdentifiable, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Kimliğe ait rol gereksinimini tanımlayan sınıf
        /// </summary>
        /// <param name="roles">Gerekli roller</param>
        /// <param name="identifier">Kimlik tanımayıcı</param>
        public RolesRequirement(string roles, Guid identifier)
        {
            Roles = roles;
            Identifier = identifier;
        }

        /// <summary>
        /// Gerekli roller
        /// </summary>
        public string Roles { get; }

        /// <summary>
        /// Kimlik tanımayıcı
        /// </summary>
        public Guid Identifier { get; set; }

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

                }

                disposed = true;
            }
        }
    }
}
