using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Services.Api.Infrastructure.Authorization.Entities.EntityFramework
{
    /// <summary>
    /// Kullanıcının modeli
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Kullanıcının e-posta adresi
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Kullanıcının parolası
        /// </summary>
        public string Password { get; set; }

        public virtual ICollection<Claim> Claims { get; set; } = new Collection<Claim>();
        public virtual ICollection<Session> Sessions { get; set; } = new Collection<Session>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new Collection<UserRole>();
    }
}
