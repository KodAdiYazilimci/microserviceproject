namespace Infrastructure.Communication.Http.Models
{
    public class HttpHeaderModel
    {
        public string Name { get; set; }
        public bool Required { get; set; }
        public string Value { get; set; }
    }
}
