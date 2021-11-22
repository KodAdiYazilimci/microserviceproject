using System.Collections.Generic;

namespace Services.Communication.Http.Broker.Authorization.Models
{
    /// <summary>
    /// Yetki poliçeleri sınıfı
    /// </summary>
    public class PolicyModel
    {
        /// <summary>
        /// Poliçenin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Poliçenin rolleri
        /// </summary>
        public List<RoleModel> Roles { get; set; } = new List<RoleModel>();
    }
}
