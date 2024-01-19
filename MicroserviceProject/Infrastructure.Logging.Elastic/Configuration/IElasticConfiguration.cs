namespace Infrastructure.Logging.Elastic.Configuration
{
    public interface IElasticConfiguration
    {
        string Host { get; }
        string UserName { get; }
        string Password { get; }
        string Index { get; }
    }
}
