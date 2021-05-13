using Infrastructure.Sockets.Persistence.Repositories.Sql;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Factories
{
    public class SocketRepositoryFactory
    {
        private static SocketRepository socketRepository;

        public static SocketRepository GetSocketRepository(IConfiguration configuration)
        {
            if (socketRepository == null)
            {
                socketRepository = new SocketRepository(configuration);
            }

            return socketRepository;
        }
    }
}
