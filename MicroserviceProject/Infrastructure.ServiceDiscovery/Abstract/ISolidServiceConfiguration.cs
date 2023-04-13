namespace Infrastructure.ServiceDiscovery.Abstract
{
    public interface ISolidServiceConfiguration
    {
        long ExpirationServiceInfo { get; }
        bool OverrideDnsName { get; }
        string OverridenDnsName { get; }
    }
}
