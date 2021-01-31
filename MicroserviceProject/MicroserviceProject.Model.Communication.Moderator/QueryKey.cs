using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceProject.Model.Communication.Moderator
{
    /// <summary>
    /// Çağırılacak servisin query string parametreleri
    /// </summary>
    public class QueryKey
    {
        [Key]
        public int Id { get; set; }
        public int CallModelId { get; set; }
        public string Key { get; set; }

        /// <summary>
        /// Parametrenin ait olduğu çağrı modeli
        /// </summary>
        [JsonIgnore]
        [ForeignKey(nameof(CallModelId))]
        public virtual CallModel CallModel { get; set; }
    }
}
