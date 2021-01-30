namespace MicroserviceProject.Model.Security
{
    public class Credential
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
        public string Region { get; set; }
    }
}
