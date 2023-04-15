namespace Infrastructure.ServiceDiscovery.Register.Abstract
{
    public interface IRegisterationConfiguration
    {
        bool OverrideDnsName { get; }
        string OverridenDnsName { get; }
        string ServiceName { get; }
        int Port { get; }
        string Protocol { get; }
    }
}
