using System.Collections.Generic;

namespace Infrastructure.Security.Model
{
    /// <summary>
    /// Yetki poliçeleri sınıfı
    /// </summary>
    public class AuthorizationPolicy
    {
        /// <summary>
        /// Poliçenin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Poliçenin rolleri
        /// </summary>
        public List<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
