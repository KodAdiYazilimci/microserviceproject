using Infrastructure.Security.Providers;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Factories
{
    public class CredentialProviderFactory
    {
        private static CredentialProvider credentialProvider;

        public static CredentialProvider GetCredentialProvider(IConfiguration configuration)
        {
            if (credentialProvider == null)
            {
                credentialProvider = new CredentialProvider(configuration);
            }

            return credentialProvider;
        }
    }
}
