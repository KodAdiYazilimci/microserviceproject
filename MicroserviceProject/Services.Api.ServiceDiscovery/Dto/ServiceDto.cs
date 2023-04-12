namespace Services.Api.ServiceDiscovery.Dto
{
    public class ServiceDto
    {
        public string ServiceName { get; set; }
        public string Protocol { get; set; }
        public int Port { get; set; }
        public new List<EndpointDto> Endpoints { get; set; }
    }
}
