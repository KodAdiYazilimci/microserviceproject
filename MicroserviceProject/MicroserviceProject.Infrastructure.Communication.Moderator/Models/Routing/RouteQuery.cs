namespace MicroserviceProject.Infrastructure.Communication.Moderator.Models.Routing
{
    /// <summary>
    /// Çağırılacak servisin query string parametreleri
    /// </summary>
    public class RouteQuery
    {
        public int Id { get; set; }
        public int CallModelId { get; set; }
        public string Key { get; set; }
    }
}
