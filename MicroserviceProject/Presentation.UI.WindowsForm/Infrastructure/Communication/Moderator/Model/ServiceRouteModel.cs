using System.Collections.Generic;

namespace Presentation.UI.Infrastructure.Communication.Broker.Model
{
    /// <summary>
    /// Çağrı modeli
    /// </summary>
    public class ServiceRouteModel
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
        /// Bu servis endpoint ile başarısız iletişim kurulması halinde denenecek diğer endpointler
        /// </summary>
        public List<ServiceRouteModel> AlternativeRoutes { get; set; }

        /// <summary>
        /// Çağırılacak servisin query string parametreleri
        /// </summary>        
        public virtual ICollection<RouteQuery> QueryKeys { get; set; } = new List<RouteQuery>();
    }
}
