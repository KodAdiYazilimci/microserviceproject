namespace Infrastructure.Routing.Models
{
    /// <summary>
    /// Çağırılacak servisin query string parametreleri
    /// </summary>
    public class RouteQueryModel
    {
        public int Id { get; set; }
        public int CallModelId { get; set; }
        public string Key { get; set; }
    }
}
