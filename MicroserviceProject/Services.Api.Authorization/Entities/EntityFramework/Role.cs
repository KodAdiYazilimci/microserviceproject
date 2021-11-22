using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Services.Api.Infrastructure.Authorization.Entities.EntityFramework
{
    /// <summary>
    /// Kullanıcı rolü sınıfı
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Rolün adı
        /// </summary>
        public string Name { get; set; }

        public virtual ICollection<PolicyRole> PolicyRoles { get; set; } = new Collection<PolicyRole>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new Collection<UserRole>();
    }
}
