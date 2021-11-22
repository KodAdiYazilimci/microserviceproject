using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Services.Api.Infrastructure.Authorization.Entities.EntityFramework
{
    /// <summary>
    /// Yetki poliçeleri sınıfı
    /// </summary>
    public class Policy : BaseEntity
    {
        /// <summary>
        /// Poliçenin adı
        /// </summary>
        public string Name { get; set; }

        public virtual ICollection<PolicyRole> PolicyRoles { get; set; } = new Collection<PolicyRole>();
    }
}
