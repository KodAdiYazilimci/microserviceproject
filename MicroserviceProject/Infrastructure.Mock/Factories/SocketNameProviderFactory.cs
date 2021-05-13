using Infrastructure.Sockets.Providers;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Factories
{
    public class SocketNameProviderFactory
    {
        private static SocketNameProvider socketNameProvider;

        public static SocketNameProvider GetSocketNameProvider(IConfiguration configuration)
        {
            if (socketNameProvider == null)
            {
                socketNameProvider = new SocketNameProvider(configuration);
            }

            return socketNameProvider;
        }
    }
}
