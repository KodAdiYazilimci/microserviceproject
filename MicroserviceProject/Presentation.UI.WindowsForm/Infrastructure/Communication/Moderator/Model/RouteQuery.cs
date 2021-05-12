namespace MicroserviceProject.Presentation.UI.Infrastructure.Communication.Moderator.Model
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
