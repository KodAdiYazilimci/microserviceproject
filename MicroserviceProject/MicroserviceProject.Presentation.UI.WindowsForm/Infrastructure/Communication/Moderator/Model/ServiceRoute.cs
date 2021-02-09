using System.Collections.Generic;

namespace MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator.Model
{
    /// <summary>
    /// Çağrı modeli
    /// </summary>
    public class ServiceRoute
    {
        public int Id { get; set; }
        /// <summary>
        /// Çağırılacak servisin adı
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Çağırılacak servisin çağrı tipi (Post,Get)
        /// </summary>
        public string CallType { get; set; }

        /// <summary>
        /// Çağırılacak servisin urli
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Çağırılacak servisin query string parametreleri
        /// </summary>        
        public virtual ICollection<RouteQuery> QueryKeys { get; set; } = new List<RouteQuery>();
    }
}
