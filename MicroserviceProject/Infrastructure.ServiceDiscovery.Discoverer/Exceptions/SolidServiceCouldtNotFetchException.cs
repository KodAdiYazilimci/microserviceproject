namespace Infrastructure.ServiceDiscovery.Discoverer.Exceptions
{
    public class SolidServiceCouldtNotFetchException : Exception
    {
        public SolidServiceCouldtNotFetchException() : base("Solid service couldn't fetch")
        {

        }

        public SolidServiceCouldtNotFetchException(string message) : base(message)
        {

        }
    }
}
