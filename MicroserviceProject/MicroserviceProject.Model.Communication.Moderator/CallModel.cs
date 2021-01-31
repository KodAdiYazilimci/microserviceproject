using System.Collections.Generic;

namespace MicroserviceProject.Model.Communication.Moderator
{
    /// <summary>
    /// Çağrı modeli
    /// </summary>
    public class CallModel
    {
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
        public List<string> QueryKeys { get; set; }
    }
}
