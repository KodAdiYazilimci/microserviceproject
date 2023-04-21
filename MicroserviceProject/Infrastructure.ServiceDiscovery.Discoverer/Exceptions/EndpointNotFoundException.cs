namespace Infrastructure.ServiceDiscovery.Discoverer.Exceptions
{
    public class EndpointNotFoundException : Exception
    {
        public EndpointNotFoundException() : base("Endpoint not found")
        {

        }

        public EndpointNotFoundException(string message) : base(message)
        {

        }
    }
}
