using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Services.Api.Authorization.Entities.EntityFramework
{
    /// <summary>
    /// Kullanıcı nitelik tipleri
    /// </summary>
    public class ClaimType : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Claim> Claims { get; set; } = new Collection<Claim>();
    }
}
